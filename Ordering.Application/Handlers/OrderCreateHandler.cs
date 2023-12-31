﻿using AutoMapper;
using MediatR;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class OrderCreateHandler : IRequestHandler<OrderCreateCommand, OrderResponse> //mediatr.Send() methoduna a OrderCreateCommand tipinde model geldiğinde OrderCreateHandler calısır ve asagıdaki Handle işlemini uygular.
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderCreateHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);   //mapping hataları almamak icin OrderMappingProfile class ı olusturuldu. DependencyInjectionda da bu OrderMappingProfile inject edildi.

            if (orderEntity == null)            
                throw new ApplicationException("Entity could not be mapped!");            

            var order = await _orderRepository.AddAsync(orderEntity);
            
            var orderResponse = _mapper.Map<OrderResponse>(order);

            return orderResponse;
        }
    }
}
