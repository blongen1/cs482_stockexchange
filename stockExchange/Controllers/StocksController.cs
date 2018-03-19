using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToRoute(new {controller = "Account", action = "Login"});
            }

            var stocks = _context.Stocks.ToList();

            return View(stocks);
        }

        // Get: Stocks/Details/{id}
        public ActionResult Details(string symbol)
        {

            if (!Request.IsAuthenticated)
            {
                return RedirectToRoute(new {controller = "Account", action = "Login"});
            }

            var stocks = _context.Stocks.SingleOrDefault(c => c.Symbol == symbol);

            if (stocks == null)
            {
                return HttpNotFound();
            }

            var amountOwned = _context.Portfolios.ToList().Where(t => t.UserId == User.Identity.GetUserId())
                                  .Where(t => t.Symbol == stocks.Symbol).Where(t => t.TransactionType == "Buy")
                                  .Sum(t => t.Amount)
                              - _context.Portfolios.ToList().Where(t => t.UserId == User.Identity.GetUserId()).Where(
                                      t => t.Symbol == stocks.Symbol).Where(t => t.TransactionType == "Sell")
                                  .Sum(t => t.Amount);

            var viewModel = new DetailsViewModel
            {
                Stocks = stocks,
                AmountOwned = amountOwned
            };

            return View(viewModel);
        }

        public static void UpdateStockPrices()
        {
            System.Diagnostics.Debug.WriteLine("Hello, world!");
        }
    }
}
