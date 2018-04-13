using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ActionResult Details(string symbol, string timeperiod)
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

            var dayPrices = new List<double>();
            int year, month, day = 0;

            var retrievedDate = DateTime.Now;

            // If the current time is before 10am
            if (DateTime.Now.Hour < 10)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    // Use Friday's date
                    retrievedDate = retrievedDate.AddDays(-3);
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    // Use Friday's date
                    retrievedDate = retrievedDate.AddDays(-2);
                }
                else
                {
                    // Use yesterday's date
                    retrievedDate = retrievedDate.AddDays(-1);
                }
            }
            else {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    retrievedDate = retrievedDate.AddDays(-2);
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                {
                    retrievedDate = retrievedDate.AddDays(-1);
                }
            }

            if (timeperiod == "yesterday")
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    year = retrievedDate.AddDays(-3).Year;
                    month = retrievedDate.AddDays(-3).Month;
                    day = retrievedDate.AddDays(-3).Day;
                }
                else
                {
                    year = retrievedDate.AddDays(-1).Year;
                    month = retrievedDate.AddDays(-1).Month;
                    day = retrievedDate.AddDays(-1).Day;
                }
            }
            else
            {
                year = retrievedDate.Year;
                month = retrievedDate.Month;
                day = retrievedDate.Day;
            }

            var prices = _context.PriceHistory.Where(t => t.Symbol == symbol).Where(t => t.Time.Year == year && t.Time.Month == month && t.Time.Day == day);

            foreach (var p in prices)
            {
                dayPrices.Add(p.Price);
            }

            var viewModel = new DetailsViewModel
            {
                Stocks = stocks,
                AmountOwned = amountOwned,
                DayPrices = dayPrices,
                TimePeriod = timeperiod

            };

            return View(viewModel);
        }

        public void UpdateStockPrices()
        {

            string csvData;

            using (WebClient web = new WebClient())
            {
                csvData = web.DownloadString("https://docs.google.com/spreadsheets/d/e/2PACX-1vQLPVN5opgmUgOQ3W1JNJQ6VrjxIwtIXVGE_v4wzLUovFpXqG0G1w3wrBM8lXqV1VDKJKL3kholtuRI/pub?output=csv");
            }

            var stocks = _context.Stocks.ToList();

            string[] rows = csvData.Replace("\r", "").Split('\n');


            foreach (var s in stocks)
            {

                string[] cols = rows[s.Id-12].Split(',');

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

                    // (Sometimes these values are null when the market isn't open yet).
                }

            }

            _context.SaveChanges();

        }

        public void BuildPriceHistory()
        {
            // If the market is currently open
            if ((DateTime.Now.Hour == 9 && DateTime.Now.Minute >= 30 || DateTime.Now.Hour > 9 && DateTime.Now.Hour < 16 || DateTime.Now.Hour == 16 && DateTime.Now.Minute < 15) 
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

        public void CheckPriceAlert()
        {

            MailMessage mailMessage = new MailMessage("b.longenecker2@gmail.com", "b.longenecker7@gmail.com");
            mailMessage.Subject = "Hello World!";
            mailMessage.Body = @"
                        <html lang=""en"">
                            <head>    
                                
                            </head>
                            <body>
                                " + _context.Stocks.ToList().Single(t => t.Symbol == "NVDA").CompanyName +
                               @" has increased by 10% today!
                            </body>
                        </html>
                        ";

            mailMessage.IsBodyHtml = true;


            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

        }
    }
}
