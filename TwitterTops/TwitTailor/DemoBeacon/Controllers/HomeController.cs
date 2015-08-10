using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoBeacon.Models;

namespace DemoBeacon.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetLog()
        {
            IList<tblDemoBeacon> listObj = null;
            try
            {
                //listObj = new List<tblDemoBeacon>();
                listObj = new TwitRapDBEntities().tblDemoBeacons.ToList().OrderByDescending(x => x.LogTime).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            return PartialView("LogsView", listObj);
        }
    }
}
