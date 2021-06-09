using Microsoft.AspNetCore.Mvc;
using StockAnalyzer.Core;
using System;
using System.Threading.Tasks;

namespace StockAnalyzer.Web.Controllers
{
    public class StocksController : Controller
    {
        [Route("api/stocks/{ticker}")] 
        public async Task<IActionResult> Get(string ticker)
        {
            //var store = new DataStore(HostingEnvironment.MapPath("~/bin"));

            var store = new DataStore(Environment.CurrentDirectory);
            var data = await store.LoadStocks();

            if (!data.ContainsKey(ticker)) return NotFound();

            return Json(data[ticker]);
        }
    }
}