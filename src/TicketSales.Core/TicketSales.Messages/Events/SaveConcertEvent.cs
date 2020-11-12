using System;

namespace TicketSales.Messages.Events
{
    public class SaveConcertEvent
    {
        public Guid MessageId { get; set; }

        public bool Accepted { get; set; }
    }
}
