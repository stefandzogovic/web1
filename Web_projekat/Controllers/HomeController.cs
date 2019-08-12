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
            Admini admini = (Admini)HttpContext.Application["admini"];
            ViewBag.loggedin = false;
            User sc = (User)Session["user"];

            Users users = (Users)Session["users"];
            if (users == null)
            {   
                users = new Users();
                Session["users"] = users;
            }

            if (Session["user"] != null)
            {
                ViewBag.user = sc;
            }
            return View();
        }
    }
}