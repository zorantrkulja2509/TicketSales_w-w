using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSales.Core.Domain.Entities
{
    public class Tickets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Concert")]
        public int ConcertId { get; set; }
        public int NumberOfTickets { get; set; }

        public virtual User User { get; set; }
        public virtual Concert Concert { get; set; }
    }
}
