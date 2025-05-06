using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReserva.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Tu aplicación descripción página.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Tu página de contacto.";
            return View();
        }
    }
}