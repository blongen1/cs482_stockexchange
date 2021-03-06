﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using stockExchange.Models;

namespace stockExchange.ViewModels
{
    public class PortfolioViewModel
    {
        public List<Portfolio> Portfolio { get; set; }
        public List<double> CurrentPrice { get; set; }
        public List<double> AssetValue { get; set; }
        public List<double> GainLoss { get; set; }
        public double TotalValue { get; set; }
        public List<string> SymbolList { get; set; }
    }
}