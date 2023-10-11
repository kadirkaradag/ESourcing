using MediatR;
using Ordering.Application.Responses;
using System;

namespace Ordering.Application.Commands.OrderCreate
{
    public class OrderCreateCommand : IRequest<OrderResponse>  // OrderCreateCommand tipinde bir emir aldıgında geri dönüşte OrderResponse tipinde bir nesne dönmesi gerekiyor
    {
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
