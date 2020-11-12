using System.Collections.Generic;

namespace TicketSales.Messages.Events
{
    public class GetTicketByUserIdEvent
    {
        public List<GetTicketEvent> TicketViewModel { get; set; }
    }
}
