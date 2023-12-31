﻿using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESourcing.UI.Clients
{
    public class BidClient
    {
        public HttpClient _client { get; }

        public BidClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<List<BidViewModel>>> GetAllBidsByAuctionId(string id)
        {
            var response = await _client.GetAsync("/Bid/BidsByAuction/"+id);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<BidViewModel>>(responseData);
                if(result.Any())
                {
                    return new Result<List<BidViewModel>>(true,ResultConstant.RecordFound,result.ToList());
                }
                return new Result<List<BidViewModel>>(true, ResultConstant.RecordNotFound);
            }
            return new Result<List<BidViewModel>>(true, ResultConstant.RecordNotFound);

        }
        public async Task<Result<string>> SendBid(BidViewModel model)
        {
            var dataAsString = JsonConvert.SerializeObject(model);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/Bid", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return new Result<string>(true, ResultConstant.RecordCreated, responseData);
            }
            return new Result<string>(false, ResultConstant.RecordNotCreated);
        }

    }
}
