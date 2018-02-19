using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stockExchange.Models;
using stockExchange.ViewModels;

namespace stockExchange.Controllers
{
    public class PortfolioController : Controller
    {

        private ApplicationDbContext _context;

        public PortfolioController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Portfolio
        public ActionResult Index()
        {

            var portfolio = new Portfolio() {Symbol = "NVDA"};

            return View(portfolio);
        }

        [HttpPost]
        public ActionResult Buy(Portfolio portfolio)
        {

            _context.Portfolios.Add(portfolio);
            //_context.SaveChanges();


            return RedirectToAction("Index", "Portfolio");
        }

    }
}