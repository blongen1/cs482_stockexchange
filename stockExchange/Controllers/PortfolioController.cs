using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stockExchange.Models;

namespace stockExchange.Controllers
{
    public class PortfolioController : Controller
    {
        // GET: Portfolio
        public ActionResult Index()
        {

            var portfolio = new Portfolio() {Symbol = "NVDA"};

            return View(portfolio);
        }
    }
}