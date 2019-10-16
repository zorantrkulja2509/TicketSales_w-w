using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSales.User.Models;
using TicketSales.User.Services;

namespace TicketSales.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestMessageStore _store;

        public HomeController(TestMessageStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public IActionResult Index()
        {
            int count = _store.GetCount();
            return View(count);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
