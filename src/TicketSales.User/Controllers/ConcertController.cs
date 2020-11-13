using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertController : Controller
    {
        public readonly IServiceProvider _serviceProvider;
        private readonly IRequestClient<GetAllConcertsCommand> _requestClient;
        private readonly IRequestClient<BuyTicketCommand> _requestClientBuyTicketCommand;

        public ConcertController(IServiceProvider serviceProvider,
                                 IRequestClient<GetAllConcertsCommand> requestClient,
                                 IRequestClient<BuyTicketCommand> requestClientBuyTicketCommand)
        {
            _serviceProvider = serviceProvider;
            _requestClient = requestClient;
            _requestClientBuyTicketCommand = requestClientBuyTicketCommand;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var request = _requestClient.Create(new GetAllConcertsCommand());
            var response = await request.GetResponse<GetAllConcertsEvent>();

            return View(response.Message.ConcertViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BuyTickets([FromForm] BuyTicketCommand buyTicketCommand)
        {
            var request = _requestClientBuyTicketCommand.Create(buyTicketCommand);
            await request.GetResponse<BuyTicketsEvent>();

            return RedirectToAction("Index");
        }
    }
}