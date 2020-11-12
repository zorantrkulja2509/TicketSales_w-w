using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Core.Domain.Entities;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Application
{
    public class SaveConcertCommandHandler : IConsumer<SaveConcertCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SaveConcertCommand> _logger;

        public SaveConcertCommandHandler(IDataContext dataContext, IMapper mapper, ILogger<SaveConcertCommand> logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<SaveConcertCommand> context)
        {
            try
            {
                var concert = _mapper
                                    .Map<Concert>(context.Message);

                _dataContext
                        .Concerts
                        .Add(concert);

                await _dataContext
                            .SaveChangesAsync()
                            .ConfigureAwait(false);

                await context
                            .Publish(concert);

                await context
                            .RespondAsync(new SaveConcertEvent()
                            {
                                Accepted = true
                            });
            }
            catch (Exception ex)
            {
                _logger.LogError("ConcertDTOError", ex);
            }
        }
    }
}
