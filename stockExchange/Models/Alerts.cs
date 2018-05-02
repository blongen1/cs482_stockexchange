using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stockExchange.Models
{
    public class Alerts
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public int PercentChange { get; set; }
    }
}