namespace TicketSales.Messages.Commands
{
    public class BuyTicketCommand
    {
        public int ConcertId { get; set; }
        public int NumberOfTickets { get; set; }
        public int UserId { get; set; }
    }
}
