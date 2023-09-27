using ESourcing.Sourcing.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ESourcing.Sourcing.Data
{
    public class SourcingContextSeed
    {
        public static void SeedData(IMongoCollection<Auction> auctionCollection)
        {
            bool isExist = auctionCollection.Find(p => true).Any();
            if (!isExist)
            {
                auctionCollection.InsertManyAsync(GetPreconfiguredAuctions());
            }
        }

        private static IEnumerable<Auction> GetPreconfiguredAuctions()
        {
            return new List<Auction>()
            {
                new Auction()
                {
                    Name ="Auction 1",
                    Description ="Description...",
                    CreatedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    FinishedAt=DateTime.Now.AddDays(10),
                    ProductId="651339e21134d8be80a0f391",
                    IncludedSellers=new List<string>()
                    {
                        "seller1@test.com",
                        "seller2@test.com",
                        "seller3@test.com"
                    },
                    Quantity=4,
                    Status=(int)Status.Active

                },
                new Auction()
                {
                    Name ="Auction 2",
                    Description ="Description.. 2.",
                    CreatedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    FinishedAt=DateTime.Now.AddDays(10),
                    ProductId="651339e21134d8be80a0f391",
                    IncludedSellers=new List<string>()
                    {
                        "seller1@test.com",
                        "seller2@test.com",
                        "seller3@test.com"
                    },
                    Quantity=4,
                    Status=(int)Status.Active

                },
                new Auction()
                {
                    Name ="Auction 3",
                    Description ="Description 3",
                    CreatedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    FinishedAt=DateTime.Now.AddDays(10),
                    ProductId="651339e21134d8be80a0f391",
                    IncludedSellers=new List<string>()
                    {
                        "seller1@test.com",
                        "seller2@test.com",
                        "seller3@test.com"
                    },
                    Quantity=4,
                    Status=(int)Status.Active

                }
            };
        }
    }
}
