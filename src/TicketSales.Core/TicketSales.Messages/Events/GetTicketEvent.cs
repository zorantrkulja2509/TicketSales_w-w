namespace TicketSales.Messages.Events
{
    public class GetTicketEvent
    {
        public int Id { get; set; }
        public string Concert { get; set; }
        public int NumberOfTickets { get; set; }
        public int ConcertId { get; set; }
    }
}
