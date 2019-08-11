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

            User sc = (User)Session["user"];
            if (sc == null)
            {
                sc = new Models.User.User();
                Session["user"] = sc;
            }

            Users users = (Users)Session["users"];
            if (users == null)
            {
                users = new Users();
                Session["users"] = users;
            }

            ViewBag.user = sc;
            return View();
        }
    }
}