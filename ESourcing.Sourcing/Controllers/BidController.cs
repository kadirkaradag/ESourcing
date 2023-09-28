using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;

        public BidController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        [HttpGet("BidsByAuction/{id:length(24)}",Name = "GetBidsByAuctionId")]
        [ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
        {
            var bids = await _bidRepository.GetBidsByAuctionId(id);
            return Ok(bids);
        }

        [HttpGet("{auctionId:length(24)}", Name = "GetWinnerBid")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string auctionId)
        {
            var bid = await _bidRepository.GetWinnerBid(auctionId);
            return Ok(bid);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendBid([FromBody] Bid bid)
        {
            await _bidRepository.SendBid(bid);
            return Ok();
        }


    }
}
