using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stockExchange.Models
{
    public class Stocks
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public double PreviousClose { get; set; }
        public double CurrentPrice { get; set; }
        public int Volume { get; set; }
        public double DayLow { get; set; }
        public double DayHigh { get; set; }
        public double YearLow { get; set; }
        public double YearHigh { get; set; }
    }
}