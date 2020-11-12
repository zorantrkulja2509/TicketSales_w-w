using AutoMapper;
using TicketSales.Core.Domain.Entities;
using TicketSales.Messages.Events;

namespace TicketSales.Messages.Mapper
{
    public class TicketMapper : Profile
    {
        public TicketMapper()
        {
            CreateMap<Tickets, GetTicketEvent>()
                .ForMember(dest => dest.Concert, o => o.MapFrom(x => x.Concert.Name))
                .ForMember(dest => dest.ConcertId, o => o.MapFrom(x => x.Concert.Id));
        }
    }
}
