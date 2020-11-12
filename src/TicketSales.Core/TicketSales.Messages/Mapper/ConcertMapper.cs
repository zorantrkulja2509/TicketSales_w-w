using AutoMapper;
using TicketSales.Core.Domain.Entities;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Messages.Mapper
{
    public class ConcertMapper : Profile
    {
        public ConcertMapper()
        {
            CreateMap<Concert, GetConcertEvent>();
            CreateMap<SaveConcertCommand, Concert>()
                .ForMember(dest => dest.NumberOfRemainingTickets, o => o.MapFrom(x => x.NumberOfTickets));
        }
    }
}
