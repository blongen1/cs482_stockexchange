using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stockExchange.Models
{
    public class PriceHistory
    {

        public int Id { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }

    }
}