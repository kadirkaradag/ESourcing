using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;

        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidsByAuctionId(string auctionId)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(m => m.AuctionId, auctionId);
            List<Bid> bids = await _context.Bids.Find(filter).ToListAsync();
            bids = bids.OrderByDescending(a => a.CreatedAt)
                       .GroupBy(a => a.SellerUserName)
                       .Select(a => new Bid
                       {
                           AuctionId = a.FirstOrDefault().AuctionId,
                           CreatedAt = a.FirstOrDefault().CreatedAt,
                           SellerUserName = a.FirstOrDefault().SellerUserName,
                           Id = a.FirstOrDefault().Id,
                           Price = a.FirstOrDefault().Price,
                           ProductId = a.FirstOrDefault().ProductId,
                           
                       })
                       .ToList();
            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> bids = await GetBidsByAuctionId(id);
            return bids.OrderByDescending(a=>a.Price).FirstOrDefault();

        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
