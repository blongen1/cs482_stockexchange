﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using stockExchange.Extensions;
using stockExchange.Models;
using stockExchange.ViewModels;

namespace stockExchange.Controllers
{
    public class PortfolioController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;

        public PortfolioController()
        {
            _context = new ApplicationDbContext();
        }

        public PortfolioController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Portfolio
        [HttpGet]
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return RedirectToRoute(new {controller = "Account", action="Login"});
            }

            var portfolio = _context.Portfolios.ToList().Where(t => t.UserId == userId);

            var symbolsPurchased = portfolio.Select(t => t.Symbol).Distinct();

            var returnPortfolio = new List<Portfolio>();
            var currentPrice = new List<double>();
            var assetValue = new List<double>();
            var gainLoss = new List<double>();
            var symbolList = new List<string>();

            double totalValue = 0;

            var id = 0;
            foreach (var s in symbolsPurchased)
            {
                var amt = _context.Portfolios.ToList().Where(t => t.UserId == userId)
                              .Where(t => t.Symbol == s).Where(t => t.TransactionType == "Buy").Sum(t => t.Amount)
                          - _context.Portfolios.ToList().Where(t => t.UserId == userId).Where(
                                  t => t.Symbol == s).Where(t => t.TransactionType == "Sell")
                              .Sum(t => t.Amount);
                if (amt > 0)
                {
                    var prc = _context.Portfolios.ToList().Where(t => t.UserId == userId).Where(t => t.Symbol == s)
                        .Where(t => t.TransactionType == "Buy").Average(t => t.Price);
                    var time = _context.Portfolios.ToList().Where(t => t.UserId == userId).Where(t => t.Symbol == s)
                        .Where(t => t.TransactionType == "Buy").Last().Time;
                    var type = "";
                    if (_context.Stocks.SingleOrDefault(t => t.Symbol == s).CurrentPrice - prc > 0)
                    {
                        type = "Gain";
                    }
                    else if (_context.Stocks.SingleOrDefault(t => t.Symbol == s).CurrentPrice - prc < 0)
                    {
                        type = "Loss";
                    }
                    else
                    {
                        type = "Unchanged";
                    }
                    returnPortfolio.Add(new Portfolio() {Amount = amt, Id = id, Price = prc, Symbol = s, Time = time, TransactionType = type, UserId = userId});
                    
                    currentPrice.Add(_context.Stocks.SingleOrDefault(t => t.Symbol == s).CurrentPrice);
                    assetValue.Add(currentPrice.Last()*amt);
                    totalValue += currentPrice.Last() * amt;
                    gainLoss.Add(assetValue.Last() - amt*prc);
                    symbolList.Add(s);

                    id += 1;

                }

            }

            assetValue.Add(Convert.ToDouble(User.Identity.GetCash()));
            symbolList.Add("Buying Power");

            var viewModel = new PortfolioViewModel()
            {
                Portfolio = returnPortfolio,
                CurrentPrice = currentPrice,
                AssetValue = assetValue,
                GainLoss = gainLoss,
                TotalValue = totalValue,
                SymbolList = symbolList
            };
            
            return View(viewModel);
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
        public async Task<ActionResult> Buy(Portfolio portfolio)
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

            var u = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (u != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Portfolio");
        }

        [HttpPost]
        public async Task<ActionResult> Sell(Portfolio portfolio)
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

            var u = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (u != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Portfolio");
        }

    }
}