using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Application
{
    public class GetTicketByUserIdCommandHandler : IConsumer<GetTicketByUserIdCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;

        public GetTicketByUserIdCommandHandler(IDataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetTicketByUserIdCommand> context)
        {
            var concerts = _dataContext
                                    .Tickets
                                    .Include(x => x.Concert)
                                    .Where(x => x.UserId == context.Message.UserId).ToList();

            await context.RespondAsync(new GetTicketByUserIdEvent()
            {
                TicketViewModel = _mapper
                                    .Map<List<GetTicketEvent>>(concerts)
            });
        }
    }
}
