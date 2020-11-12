using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Core.Domain.Entities;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Application
{
    public class BuyTicketCommandHandler : IConsumer<BuyTicketCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly ILogger<BuyTicketCommand> _logger;

        public BuyTicketCommandHandler(IDataContext dataContext, ILogger<BuyTicketCommand> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BuyTicketCommand> context)
        {
            try
            {

                var concert = _dataContext
                                        .Concerts
                                        .FirstOrDefault(x => x.Id == context.Message.ConcertId);

                concert.NumberOfRemainingTickets = concert.NumberOfRemainingTickets - context.Message.NumberOfTickets;

                _dataContext
                    .Concerts
                    .Update(concert);

                var tickets = _dataContext
                                        .Tickets
                                        .FirstOrDefault(x => x.ConcertId == context.Message.ConcertId);

                if (tickets is null)
                {
                    tickets = new Tickets()
                    {
                        ConcertId = context.Message.ConcertId,
                        UserId = context.Message.UserId,
                        NumberOfTickets = context.Message.NumberOfTickets
                    };
                    _dataContext
                        .Tickets
                        .Add(tickets);

                }
                else
                {
                    tickets.NumberOfTickets = tickets.NumberOfTickets + context.Message.NumberOfTickets;
                    _dataContext
                        .Tickets
                        .Update(tickets);
                }

                await _dataContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false);

                await context.Publish(tickets);
                await context.Publish(concert);
                await context.RespondAsync(new BuyTicketsEvent()
                {
                    Accepted = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("BuyTicketCommandError", ex);
            }
        }
    }
}
