using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using StockAnalyzer.Core.Domain;

namespace StockAnalyzer.Core
{
    public class DataStoreAsyncTPL
    {
        public static Dictionary<string, IEnumerable<StockPrice>> Stocks
            = new Dictionary<string, IEnumerable<StockPrice>>();

        private string basePath { get; }

        public DataStoreAsyncTPL(string basePath)
        {
            this.basePath = basePath;
        }

        public Dictionary<string, IEnumerable<StockPrice>> LoadStocks()
        {
            // THE BELOW LINE IS COMMENTED TO SEE TO THAT EVERY TIME THE FILE GETS READ
            //if (Stocks.Any()) return Stocks;

            var prices = GetStockPrices();

            Stocks = prices
                .GroupBy(x => x.Ticker)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());

            return Stocks;
        }

        private IList<StockPrice> GetStockPrices()
        {
            var data = new List<StockPrice>();

            string[] lines = null;
            var loadLinesTask = Task.Run(() =>
            {
                lines = File.ReadAllLines(@"StockPrices_Small.csv");
                return lines;
            });

           var test = loadLinesTask.ContinueWith(t =>
            {
                var lines = t.Result;

                foreach (var line in lines.Skip(1))
                {
                    var segments = line.Split(',');

                    for (var i = 0; i < segments.Length; i++) segments[i] = segments[i].Trim('\'', '"');
                    var price = new StockPrice
                    {
                        Ticker = segments[0],
                        TradeDate = DateTime.ParseExact(segments[1], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        Volume = Convert.ToInt32(segments[6], CultureInfo.InvariantCulture),
                        Change = Convert.ToDecimal(segments[7], CultureInfo.InvariantCulture),
                        ChangePercent = Convert.ToDecimal(segments[8], CultureInfo.InvariantCulture),
                    };
                    data.Add(price);
                }
                return data;
            });

            return test.Result;
        }
    }
}
