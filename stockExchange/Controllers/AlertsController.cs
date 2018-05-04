using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using stockExchange.Models;
using stockExchange.ViewModels;

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

            var alerts = _context.Alerts.ToList().Where(t => t.EmailAddress == User.Identity.GetUserName());

            var viewModel = new AlertsViewModel
            {
                Alertses = alerts
            };
            

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddAlert(AlertsViewModel alertModel)
        {

            if (alertModel.Alerts.PercentChange == 0)
            {
                TempData["error"] = "Your entry is invalid. Try entering a positive or negative integer";
                return RedirectToAction("Index", "Alerts");
            }
            
            var currentAlerts = _context.Alerts.Where(t => t.EmailAddress == alertModel.Alerts.EmailAddress);

            if (currentAlerts.ToList().Count >= 2)
            {
                TempData["error"] = "You can not set up more than two alerts";
                return RedirectToAction("Index", "Alerts");
            }

            if (currentAlerts.SingleOrDefault(t => t.PercentChange == alertModel.Alerts.PercentChange) == null)
            {

                _context.Alerts.Add(alertModel.Alerts);
                _context.SaveChanges();

            }
            else
            {
                TempData["error"] = "You can not enter the same value as a previously set up alert";
                return RedirectToAction("Index", "Alerts");
            }


            return RedirectToAction("Index", "Alerts");
        }

        [HttpPost]
        public ActionResult DeleteAlert(AlertsViewModel alertModel)
        {

            var alert = new Alerts() {Id = alertModel.Alerts.Id};
            _context.Entry(alert).State = EntityState.Deleted;
            _context.SaveChanges();


            return RedirectToAction("Index", "Alerts");
        }


    }
}