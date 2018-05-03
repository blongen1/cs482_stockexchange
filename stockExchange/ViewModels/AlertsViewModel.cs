using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using stockExchange.Models;

namespace stockExchange.ViewModels
{
    public class AlertsViewModel
    {
        public Alerts Alerts { get; set; }
        public IEnumerable<Alerts> Alertses { get; set; }
    }
}