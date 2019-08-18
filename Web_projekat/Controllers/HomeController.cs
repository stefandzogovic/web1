using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_projekat.Models.User;

namespace Web_projekat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Admins admini = (Admins)HttpContext.Application["admini"];
            ViewBag.loggedin = false;
            User sc = (User)Session["user"];


            if (Session["user"] != null)
            {
                ViewBag.user = sc;
            }
            return View();
        }
    }
}