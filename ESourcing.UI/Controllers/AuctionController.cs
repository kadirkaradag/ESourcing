﻿using ESourcing.Core.Repositories;
using ESourcing.UI.Clients;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ProductClient _productClient;
        private readonly AuctionClient _auctionClient;
        public AuctionController(IUserRepository userRepository, ProductClient productClient, AuctionClient auctionClient)
        {
            _userRepository = userRepository;
            _productClient = productClient;
            _auctionClient = auctionClient;
        }

        public async Task<IActionResult> Index()
        {
            var auctionList = await _auctionClient.GetAuction();

            if(auctionList.IsSuccess)
                return View(auctionList.Data);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productList = await _productClient.GetProducts();
            if (productList.IsSuccess)
            {
                ViewBag.ProductList = productList.Data;
            }

            var userList = await _userRepository.GetAllAsync();
            ViewBag.UserList = userList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuctionViewModel model)
        {
            model.Status = 1;
            model.CreatedAt = DateTime.Now;
            var createAuction = await _auctionClient.CreateAuction(model);

            if (createAuction.IsSuccess)
                return RedirectToAction("Index");

            return View(model);
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
