using System.Collections.Generic;

namespace TicketSales.Messages.Events
{
    public class GetAllConcertsEvent
    {
        public List<GetConcertEvent> ConcertViewModel { get; set; }
    }
}
