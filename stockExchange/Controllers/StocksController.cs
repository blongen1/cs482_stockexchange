﻿using System;
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
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }

            var stocks = _context.Stocks.ToList();

            return View(stocks);
        }

        // Get: Stocks/Details/{id}
        public ActionResult Details(int id)
        {

            if (!Request.IsAuthenticated)
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }

            var stocks = _context.Stocks.SingleOrDefault(c => c.Id == id);


            var viewModel = new DetailsViewModel
            {
                Stocks = stocks
            };


            return View(viewModel);
        }
    }
}
