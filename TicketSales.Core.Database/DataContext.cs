using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Core.Domain.Entities;

namespace TicketSales.Core.Database
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Concert> Concerts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TicketSalesDB.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Seed.Initialize(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
