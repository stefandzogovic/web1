using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_projekat.Models.User;

namespace Web_projekat.Controllers
{
    public class UserOperationsController : Controller
    {

        // GET: UserOperations
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registerr(User user)
        {
            Users users = (Users)HttpContext.Application["users"];

            User sc = (User)Session["user"];

                sc = user;
                Session["user"] = user;

            
            
            ViewBag.user = user;
            users.lista_usera.Add(user.username, user);

            return Redirect(Url.Content("~/"));
        }

        public ActionResult AdminViewUsers()
        {
            Users users = (Users)HttpContext.Application["users"];
            Users useri = (Users)Session["users"];

            useri = users;
            Session["users"] = useri;

            ViewBag.registrovani = useri.lista_usera.Values;
            return View();
        }

        [HttpPost]
        public ActionResult LoginCheck(string username, string password)
        {
            bool login = false;

            Users users = (Users)HttpContext.Application["users"];


            User sc = (User)Session["user"];
            Admini admini = (Admini)HttpContext.Application["admini"];
            foreach(User u in admini.admini.Values)
            {
                if(u.username == username && u.password == password)
                {
                    sc = u;
                    Session["user"] = sc;
                    ViewBag.user = sc;
                    login = true;
                    break;
                }
            }

            foreach(User u in users.lista_usera.Values)
            {
                if (u.username == username && u.password == password)
                {
                    sc = u;
                    Session["user"] = sc;
                    ViewBag.user = sc;
                    login = true;
                    break;
                }
            }

            ViewBag.users = users.lista_usera.Values;

            if (login)
            {
                return Redirect(Url.Content("~/"));

            }
            else
                return Redirect(Url.Content("~/"));

        }

        [HttpPost]
        public ActionResult AdminChangeRole(User user)
        {
            Users users = (Users)HttpContext.Application["users"]; 

            users.lista_usera[user.username].role = user.role;
            TempData["notice"] = "Successfully changed";

            return RedirectToAction("AdminViewUsers");
        }
    }
}