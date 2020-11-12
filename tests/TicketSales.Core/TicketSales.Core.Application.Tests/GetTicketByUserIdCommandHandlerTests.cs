using AutoMapper;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TicketSales.Core.Application.Tests.Database;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Core.Domain.Entities;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;
using Xunit;

namespace TicketSales.Core.Application.Tests
{
    public class GetTicketByUserIdCommandHandlerTests
    {
        private GetTicketByUserIdCommandHandler _sut;
        private Mock<IDataContext> _dataContextMock;
        private Mock<IMapper> _mapperMock;

        [TestInitialize]
        [Fact]
        public void Initialize()
        {
            _dataContextMock = new Mock<IDataContext>();
            _mapperMock = new Mock<IMapper>();

            _sut = new GetTicketByUserIdCommandHandler(_dataContextMock.Object,
                _mapperMock.Object);
        }

        private Mock<ConsumeContext<GetTicketByUserIdCommand>> _consumeCtx = new Mock<ConsumeContext<GetTicketByUserIdCommand>>();

        [Fact]
        public async void ConsumeBuyTicketCommand_PublishesTicketsEvent()
        {
            Initialize();

            _dataContextMock.Setup(x => x.Users).Returns(DataContextMock.GetQueryableMockDbSet(SeedUsersData()));
            _dataContextMock.Setup(x => x.Concerts).Returns(DataContextMock.GetQueryableMockDbSet(SeedConcertsData()));
            _dataContextMock.Setup(x => x.Tickets).Returns(DataContextMock.GetQueryableMockDbSet(SeedTicketsData()));

            _consumeCtx.Setup(x => x.Message).Returns(new GetTicketByUserIdCommand() { UserId = 2 });
            await _sut.Consume(_consumeCtx.Object);
            _consumeCtx.Verify(x => x.RespondAsync(It.IsAny<GetTicketByUserIdEvent>()), Times.AtLeastOnce);
        }

        private List<User> SeedUsersData()
        {
            return new List<User>{new User
            {
                Id = 1,
                FirstName = "TEST Admin"
            },
            new User
            {
                Id = 2,
                FirstName = "TEST User"
            }};
        }

        private List<Concert> SeedConcertsData()
        {
            return new List<Concert>{new Concert
            {
                Id = 1,
                Name = "TEST Concert",
                NumberOfTickets = 10,
                NumberOfRemainingTickets = 10
            }};
        }

        private List<Tickets> SeedTicketsData()
        {
            return new List<Tickets>{new Tickets
            {
                Id = 1,
                ConcertId = 1,
                NumberOfTickets = 5,
                UserId = 2
            }};
        }
    }
}
