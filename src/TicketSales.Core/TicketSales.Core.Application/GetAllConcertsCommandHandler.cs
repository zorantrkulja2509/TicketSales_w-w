using AutoMapper;
using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Application
{
    public class GetAllConcertsCommandHandler : IConsumer<GetAllConcertsCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllConcertsCommandHandler(IDataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetAllConcertsCommand> context)
        {
            var concerts = _dataContext
                                    .Concerts
                                    .ToList();

            await context.RespondAsync(new GetAllConcertsEvent()
            {
                ConcertViewModel = _mapper
                                        .Map<List<GetConcertEvent>>(concerts)
            });
        }
    }
}
