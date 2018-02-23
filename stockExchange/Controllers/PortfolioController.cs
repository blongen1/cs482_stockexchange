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
                if (portfolio.Amount < 0)
                {
                    TempData["error"] = "You must enter a positive number!";
                    return RedirectToAction("Details", "Stocks", new { id = stock.Id });
                }

                var newCashValue = user.Cash - portfolio.Amount * portfolio.Price;
                if (newCashValue < 0)
                {
                    TempData["error"] = "You don't have enough cash for that! \nMaximum number of stocks possible: " + Math.Floor(user.Cash/portfolio.Price);
                    return RedirectToAction("Details", "Stocks", new {id = stock.Id});
                }

                user.Cash = newCashValue;
                _context.SaveChanges();
            }
            

            try
            {
                portfolio.Time = DateTime.Now.ToString();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();


            return RedirectToAction("Index", "Portfolio");
        }

    }
}