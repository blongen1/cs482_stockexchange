using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        public void UpdateStockPrices()
        {

            string csvData;

            using (WebClient web = new WebClient())
            {
                csvData = web.DownloadString("https://docs.google.com/spreadsheets/d/e/2PACX-1vTocmWnR0qxPTjkO8xIL5TWZ9TZYJUyNAHs_oN0k6QpV0VSGuVPwdXZoiksodf7BDnN8hr9lKZXJSTC/pub?gid=0&single=true&output=csv");
            }

            var stocks = _context.Stocks.ToList();

            string[] rows = csvData.Replace("\r", "").Split('\n');


            foreach (var s in stocks)
            {

                string[] cols = rows[s.Id-1].Split(',');

                s.PreviousClose = Convert.ToDouble(cols[2]);
                s.CurrentPrice = Convert.ToDouble(cols[3]);

                if (Convert.ToInt32(cols[4]) > 0)
                {
                    s.Volume = Convert.ToInt32(cols[4]);
                }

                try
                {
                    s.DayLow = Convert.ToDouble(cols[5]);
                    s.DayHigh = Convert.ToDouble(cols[6]);
                    s.YearLow = Convert.ToDouble(cols[7]);
                    s.YearHigh = Convert.ToDouble(cols[8]);
                }
                catch (Exception ex)
                {
                    // Do nothing

                    // (Sometimes this values are null when the market isn't open yet).
                }

            }

            _context.SaveChanges();

        }

        public void BuildPriceHistory()
        {

            if ((DateTime.Now.Hour == 9 && DateTime.Now.Minute >= 30 || DateTime.Now.Hour > 9 && DateTime.Now.Hour < 16) 
                && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
               
                var stocks = _context.Stocks.ToList();

                foreach (var s in stocks)
                {
                    var p = new PriceHistory();
                    p.Price = s.CurrentPrice;
                    p.Symbol = s.Symbol;
                    p.Time = DateTime.Now;
                    _context.PriceHistory.Add(p);
                }

                _context.SaveChanges();
            }
        }

    }
}
