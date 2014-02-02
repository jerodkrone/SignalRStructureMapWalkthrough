using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;

namespace Microsoft.AspNet.SignalR.StockTicker
{
    [HubName("stockTicker")]
    public class StockTickerHub : Hub
    {
        //Change to use the interface
        private readonly IStockTicker _stockTicker;

        //Remove this constructor
        //public StockTickerHub() : this(StockTicker.Instance) { }

        //Changing this to the interface
        public StockTickerHub(IStockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stockTicker.GetAllStocks();
        }

        public string GetMarketState()
        {
            return _stockTicker.MarketState.ToString();
        }

        public void OpenMarket()
        {
            _stockTicker.OpenMarket();
        }

        public void CloseMarket()
        {
            _stockTicker.CloseMarket();
        }

        public void Reset()
        {
            _stockTicker.Reset();
        }
    }
}