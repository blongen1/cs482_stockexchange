using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using stockExchange.Models;

namespace stockExchange.Controllers
{
    public class AlertsController : Controller
    {

        private ApplicationDbContext _context;

        public AlertsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Alerts
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }

            return View();
        }
    }
}