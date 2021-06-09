using StockAnalyzer.Core;
using StockAnalyzer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace StockAnalyzer.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            #region Before loading stock data
            var watch = new Stopwatch();
            watch.Start();
            StockProgress.Visibility = Visibility.Visible;
            StockProgress.IsIndeterminate = true;

            Search.Content = "Cancel";
            #endregion


            //RANJIT TASK - TODO - IN THE FUTURE - AFTER FULL UNDERSTANDING THE CONCEPT
            //  Can we move the following running code to - DataStoreAsyncTPL class 
            //  Tried but failed in the first attempt



            //DataStoreAsyncTPL dataStore = new DataStoreAsyncTPL(Environment.CurrentDirectory);
            //var stocksDictionary = dataStore.LoadStocks();
            //Stocks.ItemsSource = stocksDictionary[Ticker.Text];

            var loadLinesTask = Task.Run(() =>
            {
                var lines = File.ReadAllLines(@"StockPrices_Big.csv");
                return lines;
            });
            var processStocksTask = loadLinesTask.ContinueWith(t =>
            {
                var lines = t.Result;

                var data = new List<StockPrice>();

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

                Dispatcher.Invoke(() =>
                {
                    Stocks.ItemsSource = data.Where(price => price.Ticker == Ticker.Text);

                });
            });

            //RANJIT TASK - TODO - How to fix the following issue
            processStocksTask.ContinueWith(_ => {
                Dispatcher.Invoke(() =>
                {
                    #region After stock data is loaded
                    StocksStatus.Text = $"Loaded stocks for {Ticker.Text} in {watch.ElapsedMilliseconds}ms";
                    StockProgress.Visibility = Visibility.Hidden;
                    Search.Content = "Search";
                    #endregion
                });
            });
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));

            e.Handled = true;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
