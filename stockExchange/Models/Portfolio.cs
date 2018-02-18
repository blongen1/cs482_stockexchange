using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stockExchange.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}