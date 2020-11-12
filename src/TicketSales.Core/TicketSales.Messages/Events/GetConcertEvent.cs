namespace TicketSales.Messages.Events
{
    public class GetConcertEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfTickets { get; set; }
        public int NumberOfRemainingTickets { get; set; }
        public int UserId { get; set; }
    }
}
