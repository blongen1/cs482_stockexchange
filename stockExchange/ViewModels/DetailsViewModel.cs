using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using stockExchange.Models;

namespace stockExchange.ViewModels
{
    public class DetailsViewModel
    {
        public Stocks Stocks { get; set; }
        public Portfolio Portfolio { get; set; }
        public int AmountOwned { get; set; }
        public List<double> DayPrices { get; set; }
    }
}