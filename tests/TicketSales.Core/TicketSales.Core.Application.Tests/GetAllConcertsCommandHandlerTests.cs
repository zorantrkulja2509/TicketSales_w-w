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
    public class GetAllConcertsCommandHandlerTests
    {
        private GetAllConcertsCommandHandler _sut;
        private Mock<IDataContext> _dataContextMock;
        private Mock<IMapper> _mapperMock;

        [TestInitialize]
        [Fact]
        public void Initialize()
        {
            _dataContextMock = new Mock<IDataContext>();
            _mapperMock = new Mock<IMapper>();

            _sut = new GetAllConcertsCommandHandler(_dataContextMock.Object,
                _mapperMock.Object);
        }

        private Mock<ConsumeContext<GetAllConcertsCommand>> _consumeCtx = new Mock<ConsumeContext<GetAllConcertsCommand>>();

        [Fact]
        public async void ConsumeBuyTicketCommand_PublishesTicketsEvent()
        {
            Initialize();

            _dataContextMock.Setup(x => x.Concerts).Returns(DataContextMock.GetQueryableMockDbSet(SeedConcertsData()));

            _consumeCtx.Setup(x => x.Message).Returns(new GetAllConcertsCommand());
            await _sut.Consume(_consumeCtx.Object);
            _consumeCtx.Verify(x => x.RespondAsync(It.IsAny<GetAllConcertsEvent>()), Times.AtLeastOnce);
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
    }
}
