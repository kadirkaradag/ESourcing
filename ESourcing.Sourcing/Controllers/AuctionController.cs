﻿using AutoMapper;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;
        private readonly ILogger<AuctionController> _logger;

        public AuctionController(IAuctionRepository auctionRepository, IBidRepository bidRepository, IMapper mapper, EventBusRabbitMQProducer eventBus, ILogger<AuctionController> logger)
        {
            _auctionRepository = auctionRepository;
            _bidRepository=bidRepository;
            _mapper = mapper;
            _eventBus = eventBus;
            _logger = logger;

        }

        [HttpGet]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet("{id:length(24)}", Name = "GetAuction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
            {
                _logger.LogError($"Auction with id : {id}, hasn't been found in database");
                return NotFound();
            }
            return Ok(auction);
        }

        [HttpGet("{name}", Name = "GetAuctionByName")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> GetAuctionByName(string name)
        {
            var auction = await _auctionRepository.GetAuctionByName(name);
            if (auction == null)
            {
                _logger.LogError($"Auction with name : {name}, hasn't been found in database");
                return NotFound();
            }
            return Ok(auction);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> CreateAuction([FromBody] Auction auction)
        {
            auction.Status = (int)Status.Active;
            await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAuction", new { id = auction.Id }, auction);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAuctionById(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }

        [HttpPost("CompleteAuction/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> CompleteAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if(auction == null)
            {
                return NotFound();
            }

            if (auction.Status != (int)Status.Active)
            {
                _logger.LogError("Auction can not be completed.");
                return BadRequest();

            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if(bid == null)
            {
                return NotFound();
            }

            OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
            eventMessage.Quantity = auction.Quantity;

            auction.Status = (int)Status.Closed;
            bool updateResponse = await _auctionRepository.Update(auction);

            if (!updateResponse)
            {
                _logger.LogError("Auction can not updated.");
                return BadRequest();

            }

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"ERROR Publishing integration event: {EventId} from {AppName}",eventMessage.Id,"Sourcing");
                throw;
            }

            return Accepted();

        }

        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent()
        {
            OrderCreateEvent eventMessage = new OrderCreateEvent();
            eventMessage.Quantity = 1;
            eventMessage.AuctionId = "dummy1";
            eventMessage.ProductId = "dummy_prod_1";
            eventMessage.Price = 40;
            eventMessage.SellerUserName = "test@test.com";             

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, "Sourcing");
                throw;
            }

            return Accepted(eventMessage);

        }

    }
}
