﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockExchange.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }

        [Display(Name = "Number of Shares")]
        public int Amount { get; set; }
        public string Time { get; set; }
        public string TransactionType { get; set; }
    }
}