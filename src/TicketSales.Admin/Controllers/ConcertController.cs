using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.User.Controllers
{
    public class ConcertController : Controller
    {
        public readonly IServiceProvider _serviceProvider;
        private readonly IRequestClient<GetAllConcertsCommand> _requestClient;
        private readonly IRequestClient<SaveConcertCommand> _requestClientSave;

        public ConcertController(IServiceProvider serviceProvider,
                                IRequestClient<GetAllConcertsCommand> requestClient,
                                IRequestClient<SaveConcertCommand> requestClientSave)
        {
            _serviceProvider = serviceProvider;
            _requestClient = requestClient;
            _requestClientSave = requestClientSave;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var request = _requestClient.Create(new GetAllConcertsCommand());
            var response = await request.GetResponse<GetAllConcertsEvent>();

            return View(response.Message.ConcertViewModel);
        }

        [HttpGet]
        public IActionResult AddConcert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddConcert(SaveConcertCommand concert, CancellationToken cancellationToken)
        {
            var request = _requestClientSave.Create(concert);
            await request.GetResponse<SaveConcertEvent>();
            TempData["Message"] = "New concert added";
            return RedirectToAction("Index");
        }
    }
}
