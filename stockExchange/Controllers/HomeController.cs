using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockExchange.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Stock Simulator is created for CS482 - Senior Software Project II";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Feel free to send me a message!";

            return View();
        }
    }
}