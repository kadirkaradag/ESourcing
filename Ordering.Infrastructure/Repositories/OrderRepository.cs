using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string sellerUserName)
        {
            return await GetAsync(x=> x.SellerUserName == sellerUserName);
        }

        //public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string sellerUserName)
        //{
        //    var orderList = await _dbContext.Orders.Where(x => x.SellerUserName == sellerUserName).ToListAsync();
        //    return orderList;
        //}



    }
}
