using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using stockExchange.Models;

namespace stockExchange.ViewModels
{
    public class TradeStocksViewModel
    {
        public Stocks Stocks { get; set; }
        public Portfolio Portfolio { get; set; }

    }
}