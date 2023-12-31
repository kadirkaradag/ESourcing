﻿using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ESourcing.UI.Clients
{
    public class AuctionClient 
    {
        public HttpClient _client { get; }

        public AuctionClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<List<AuctionViewModel>>> GetAuction()
        {
            //var response = await _client.GetAsync("/api/v1/auction");  //api gateway baglantısı olmadan.
            var response = await _client.GetAsync("/Auction"); //api gateway ile. ocelot.json icindeki tanımlara göre.

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<AuctionViewModel>>(responseData);
                if (result.Any())
                {
                    return new Result<List<AuctionViewModel>>(true, ResultConstant.RecordFound, result.ToList());
                }

                return new Result<List<AuctionViewModel>>(false, ResultConstant.RecordNotFound);

            }
            return new Result<List<AuctionViewModel>>(false, ResultConstant.RecordNotFound);

        }

        public async Task<Result<AuctionViewModel>> CreateAuction(AuctionViewModel model)
        {
            var dataAsString = JsonConvert.SerializeObject(model); // post ve get methodları icin biraz farklı işlemler yapılıyor. burası post yukarırdaki GetAuction get methoduna  vuruyor.
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/Auction", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);
                if (result != null)
                    return new Result<AuctionViewModel>(true, ResultConstant.RecordCreated, result);
                else
                    return new Result<AuctionViewModel>(false, ResultConstant.RecordNotCreated);
            }
            return new Result<AuctionViewModel>(false, ResultConstant.RecordNotCreated);
        }

        public async Task<Result<AuctionViewModel>> GetAuctionById(string id)
        {
            var response = await _client.GetAsync("/Auction/" + id);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);
                if (result != null)
                    return new Result<AuctionViewModel>(true, ResultConstant.RecordFound, result);
                else
                    return new Result<AuctionViewModel>(false, ResultConstant.RecordNotFound);

            }           
            return new Result<AuctionViewModel>(false, ResultConstant.RecordNotFound);
        }

        public async Task<Result<string>> CompleteAuction(string id)
        {
            var dataAsString = JsonConvert.SerializeObject(id);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/Auction/CompleteAuction/"+id, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return new Result<string>(true, ResultConstant.RecordCreated, responseData);
            }
            return new Result<string>(false, ResultConstant.RecordNotCreated);
        }
    }
}
