using System;

namespace TicketSales.Messages.Events
{
    public class BuyTicketsEvent
    {
        public Guid MessageId { get; set; }

        public bool Accepted { get; set; }
    }
}
