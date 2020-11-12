namespace TicketSales.Messages.Commands
{
    public class SaveConcertCommand
    {
        public string Name { get; set; }
        public int NumberOfTickets { get; set; }
        public int NumberOfRemainingTickets { get; set; }
    }
}
