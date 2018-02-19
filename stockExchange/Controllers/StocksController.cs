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
    public class StocksController : Controller
    {
        private ApplicationDbContext _context;

        public StocksController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Stocks
        public ViewResult Index()
        {
            var stocks = _context.Stocks.ToList();

            return View(stocks);
        }

        // Get: Stocks/Details/{id}
        public ActionResult Details(int id)
        {
            var stocks = _context.Stocks.SingleOrDefault(c => c.Id == id);
            var portfolio = _context.Portfolios.SingleOrDefault();

            var viewModel = new TradeStocksViewModel
            {
                Stocks = stocks,
                Portfolio = portfolio
            };


            return View(viewModel);
        }
    }
}
