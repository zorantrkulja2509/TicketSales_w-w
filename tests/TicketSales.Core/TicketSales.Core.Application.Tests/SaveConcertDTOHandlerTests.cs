using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using TicketSales.Core.Application.Tests.Database;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Core.Domain.Entities;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;
using Xunit;

namespace TicketSales.Core.Application.Tests
{
    public class SaveConcertDTOHandlerTests
    {
        private SaveConcertCommandHandler _sut;
        private Mock<IDataContext> _dataContextMock;
        private Mock<ILogger<SaveConcertCommand>> _loggerMock;
        private Mock<IMapper> _mapperMock;

        [TestInitialize]
        [Fact]
        public void Initialize()
        {
            _dataContextMock = new Mock<IDataContext>();
            _loggerMock = new Mock<ILogger<SaveConcertCommand>>();
            _mapperMock = new Mock<IMapper>();

            _sut = new SaveConcertCommandHandler(_dataContextMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        private Mock<ConsumeContext<SaveConcertCommand>> _consumeCtx = new Mock<ConsumeContext<SaveConcertCommand>>();

        [Fact]
        public async void ConsumeBuyTicketCommand_PublishesTicketsEvent()
        {
            Initialize();

            _dataContextMock.Setup(x => x.Concerts).Returns(DataContextMock.GetQueryableMockDbSet(SeedConcertsData()));

            _consumeCtx.Setup(x => x.Message).Returns(new SaveConcertCommand() { Name = "test concert", NumberOfTickets = 10 });
            var test = new Concert
            {
                Name = "test concert",
                NumberOfTickets = 10,
                NumberOfRemainingTickets = 10
            };
            _mapperMock.Setup(x => x.Map<Concert>(It.IsAny<SaveConcertCommand>())).Returns(test);
            await _sut.Consume(_consumeCtx.Object);
            _consumeCtx.Verify(x => x.Publish(It.IsAny<Concert>(), default(CancellationToken)));
            //srediti kasnije
            _consumeCtx.Verify(x => x.RespondAsync(It.IsAny<SaveConcertEvent>()), Times.AtLeastOnce);
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
