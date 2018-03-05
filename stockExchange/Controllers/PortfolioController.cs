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

            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return RedirectToRoute(new {controller = "Account", action="Login"});
            }

            var portfolio = _context.Portfolios.ToList().Where(t => t.UserId == userId);

            return View(portfolio);
        }

        // GET: Portfolio/Transactions
        public ActionResult Transactions()
        {

            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }

            var portfolio = _context.Portfolios.ToList().Where(t => t.UserId == userId);

            return View(portfolio);
        }


        [HttpPost]
        public ActionResult Buy(Portfolio portfolio)
        {

            if (!Request.IsAuthenticated)
            {
                return HttpNotFound();
            }

            var userid = User.Identity.GetUserId();

            var stock = _context.Stocks.ToList().SingleOrDefault(t => t.Symbol == portfolio.Symbol);
            
            portfolio.UserId = userid;

            var user = _context.Users.SingleOrDefault(t => t.Id == userid);

            if (user != null)
            {
                if (portfolio.Amount <= 0)
                {
                    TempData["error"] = "You must enter a positive number!";
                    return RedirectToAction("Details", "Stocks", new { symbol = stock.Symbol });
                }

                var newCashValue = user.Cash - portfolio.Amount * portfolio.Price;
                if (newCashValue < 0)
                {
                    TempData["error"] = "You don't have enough cash for that! \nMaximum number of stocks possible: " + Math.Floor(user.Cash/portfolio.Price);
                    return RedirectToAction("Details", "Stocks", new { symbol = stock.Symbol });
                }

                user.Cash = newCashValue;
                
            }
            

            try
            {
                portfolio.Time = DateTime.Now.ToString();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            portfolio.TransactionType = "Buy";
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            return RedirectToAction("Index", "Portfolio");
        }

        [HttpPost]
        public ActionResult Sell(Portfolio portfolio)
        {

            if (!Request.IsAuthenticated)
            {
                return HttpNotFound();
            }

            var userid = User.Identity.GetUserId();

            var stock = _context.Stocks.ToList().SingleOrDefault(t => t.Symbol == portfolio.Symbol);

            portfolio.UserId = userid;

            var user = _context.Users.SingleOrDefault(t => t.Id == userid);

            if (user != null)
            {
                if (portfolio.Amount <= 0)
                {
                    TempData["error"] = "You must enter a positive number!";
                    return RedirectToAction("Details", "Stocks", new { symbol = stock.Symbol });
                }

                var amountOwned = _context.Portfolios.ToList().Where(t => t.UserId == User.Identity.GetUserId())
                                      .Where(t => t.Symbol == portfolio.Symbol).Where(t => t.TransactionType == "Buy")
                                      .Sum(t => t.Amount)
                                  - _context.Portfolios.ToList().Where(t => t.UserId == User.Identity.GetUserId()).Where(
                                          t => t.Symbol == portfolio.Symbol).Where(t => t.TransactionType == "Sell")
                                      .Sum(t => t.Amount);

                if (amountOwned < portfolio.Amount)
                {
                    TempData["error"] = "You only own " + amountOwned + " stocks of " + stock.CompanyName +"!";
                    return RedirectToAction("Details", "Stocks", new { symbol = stock.Symbol });
                }

                try
                {
                    portfolio.Time = DateTime.Now.ToString();
                }
                catch (Exception)
                {
                    return HttpNotFound();
                }

                user.Cash += portfolio.Amount * portfolio.Price;

            }


            try
            {
                portfolio.Time = DateTime.Now.ToString();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            portfolio.TransactionType = "Sell";
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();


            return RedirectToAction("Index", "Portfolio");
        }

    }
}