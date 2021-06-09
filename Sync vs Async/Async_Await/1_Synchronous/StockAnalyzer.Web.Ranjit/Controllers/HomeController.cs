using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockAnalyzer.Core;
using StockAnalyzer.Web.Ranjit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StockAnalyzer.Web.Ranjit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        //[Route("stocks/{ticker}")]
        //public async Task<ActionResult> Stock(string ticker)
        //{
        //    if (string.IsNullOrWhiteSpace(ticker)) ticker = "MSFT";

        //    ViewBag.Title = $"Stock Details for {ticker}";

        //    //var store = new DataStore(HostingEnvironment.MapPath("~/bin"));
        //    var store = new DataStore(Environment.CurrentDirectory);

        //    var data = await store.LoadStocks();

        //    return View(data[ticker]);
        //}

        [Route("Stock/{ticker}")]
        public async Task<ActionResult> Stock(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker)) ticker = "MSFT";

            ViewBag.Title = $"Stock Details for {ticker}";

            var store = new DataStore(Environment.CurrentDirectory);

            //var store = new DataStore(HostingEnvironment.MapPath("~/bin"));

            var data = await store.LoadStocks();

            return View(data[ticker]);
        }
    }
}
