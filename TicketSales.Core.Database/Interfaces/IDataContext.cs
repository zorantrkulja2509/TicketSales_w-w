using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicketSales.Core.Domain.Entities;

namespace TicketSales.Core.Database.Interfaces
{
    public interface IDataContext : IDisposable
    {
        DbSet<User> Users { get; set; }
        DbSet<Tickets> Tickets { get; set; }
        DbSet<Concert> Concerts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
