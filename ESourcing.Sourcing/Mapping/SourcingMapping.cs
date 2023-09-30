using AutoMapper;
using ESourcing.Sourcing.Entities;
using EventBusRabbitMQ.Events;

namespace ESourcing.Sourcing.Mapping
{
    public class SourcingMapping : Profile
    {
        public SourcingMapping()
        {
            CreateMap<OrderCreateEvent,Bid>().ReverseMap();   //hem bid i OrderCreateEvent e hem de OrderCreateEvent i bid e cevirebiliriz reversemap ile
        }
    }
}
