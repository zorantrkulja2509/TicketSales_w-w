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
    public class TicketsController : Controller
    {
        public readonly IServiceProvider _serviceProvider;
        private readonly IRequestClient<GetTicketByUserIdCommand> _requestClient;

        public TicketsController(IServiceProvider serviceProvider,
                                 IRequestClient<GetTicketByUserIdCommand> requestClient)
        {
            _serviceProvider = serviceProvider;
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var request = _requestClient.Create(new GetTicketByUserIdCommand() { UserId = 2 });
            var response = await request.GetResponse<GetTicketByUserIdEvent>();

            return View(response.Message.TicketViewModel);
        }
    }
}