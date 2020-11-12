using Microsoft.EntityFrameworkCore;
using TicketSales.Core.Domain.Entities;

namespace TicketSales.Core.Database
{
    public static class Seed
    {
        public static void Initialize(ModelBuilder builder)
        {
            SeedUsers(builder);
            SeedConcerts(builder);
            SeedTickets(builder);
        }

        private static void SeedUsers(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        FirstName = "User",
                        LastName = "User"
                    }
                );

            builder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 2,
                        FirstName = "Admin",
                        LastName = "Admin"
                    }
                );
        }

        private static void SeedConcerts(ModelBuilder builder)
        {
            builder
                .Entity<Concert>()
                .HasData(
                    new Concert
                    {
                        Id = 1,
                        Name = "Test Concert 1",
                        NumberOfTickets = 20,
                        NumberOfRemainingTickets = 10
                    }
                );

            builder
                .Entity<Concert>()
                .HasData(
                    new Concert
                    {
                        Id = 2,
                        Name = "Test Concert 2",
                        NumberOfTickets = 30,
                        NumberOfRemainingTickets = 25
                    }
                );
        }

        private static void SeedTickets(ModelBuilder builder)
        {
            builder
                .Entity<Tickets>()
                .HasData(
                    new Tickets
                    {
                        Id = 1,
                        UserId = 2,
                        ConcertId = 1,
                        NumberOfTickets = 10
                    }
                );

            builder
                .Entity<Tickets>()
                .HasData(
                    new Tickets
                    {
                        Id = 2,
                        UserId = 2,
                        ConcertId = 2,
                        NumberOfTickets = 5
                    }
                );
        }
    }
}
