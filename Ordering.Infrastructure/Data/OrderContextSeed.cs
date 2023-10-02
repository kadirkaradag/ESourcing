using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
                new Order()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test@test.com",
                    UnitPrice = 10,
                    TotalPrice = 1000,
                    CreatedAt = DateTime.UtcNow,

                },
                new Order()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test2@test2.com",
                    UnitPrice = 4,
                    TotalPrice = 4000,
                    CreatedAt = DateTime.UtcNow,

                },
                new Order()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test3@test3.com",
                    UnitPrice = 5,
                    TotalPrice = 500,
                    CreatedAt = DateTime.UtcNow,

                },
            };
        }
    }
}
