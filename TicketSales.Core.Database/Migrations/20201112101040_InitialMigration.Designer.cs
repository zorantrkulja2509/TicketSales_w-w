﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicketSales.Core.Database;

namespace TicketSales.Core.Database.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201112101040_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("TicketSales.Core.Domain.Entities.Concert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfRemainingTickets");

                    b.Property<int>("NumberOfTickets");

                    b.HasKey("Id");

                    b.ToTable("Concerts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Test Concert 1",
                            NumberOfRemainingTickets = 10,
                            NumberOfTickets = 20
                        },
                        new
                        {
                            Id = 2,
                            Name = "Test Concert 2",
                            NumberOfRemainingTickets = 25,
                            NumberOfTickets = 30
                        });
                });

            modelBuilder.Entity("TicketSales.Core.Domain.Entities.Tickets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConcertId");

                    b.Property<int>("NumberOfTickets");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ConcertId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcertId = 1,
                            NumberOfTickets = 10,
                            UserId = 2
                        },
                        new
                        {
                            Id = 2,
                            ConcertId = 2,
                            NumberOfTickets = 5,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("TicketSales.Core.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "User",
                            LastName = "User"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Admin",
                            LastName = "Admin"
                        });
                });

            modelBuilder.Entity("TicketSales.Core.Domain.Entities.Tickets", b =>
                {
                    b.HasOne("TicketSales.Core.Domain.Entities.Concert", "Concert")
                        .WithMany()
                        .HasForeignKey("ConcertId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TicketSales.Core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
