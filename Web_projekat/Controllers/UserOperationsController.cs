﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_projekat.Models;
using Web_projekat.Models.User;

namespace Web_projekat.Controllers
{
    public class UserOperationsController : Controller
    {

        DataAccessLayer dal = new DataAccessLayer();

        // GET: UserOperations
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {

            return View();
        }

        public ActionResult FailedRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registerr(User user)
        {

            bool exist = false;
            Dictionary<string, User> tempdict = dal.usersdb.ToDictionary(x => x.username, x => x);

            foreach(User tempuser in tempdict.Values)
            {
                if(tempuser.username == user.username)
                {
                    exist = true;
                    ViewBag.username = tempuser.username;
                    break;
                }
            }

            if(exist)
            {
                return View("FailedRegister");
            }

            User sc = new User();
            sc.apartments = new List<Models.Apartment>();

            int cnt = dal.usersdb.Count();

            sc.UserId = cnt++;
            sc.email = user.email;
            sc.name = user.name;
            sc.username = user.username;
            sc.password = user.password;
            sc.surname = user.surname;
            sc.sex = user.sex;
            sc.role = user.role;

            Session["user"] = sc;   

            
            
            ViewBag.user = user;

            dal.usersdb.Add(user);
            dal.SaveChanges();

            return Redirect(Url.Content("~/"));
        }

        public ActionResult AdminViewUsers()
        {
            ViewBag.registrovani = dal.usersdb.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult LoginCheck(string username, string password)
        {
            bool login = false;


            User sc = (User)Session["user"];
            Admins admini = (Admins)HttpContext.Application["admini"];
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

            foreach(User u in dal.usersdb.ToList())
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

            if(!login)
            {
              return RedirectToAction("FailedLogin");
            }

            ViewBag.users = dal.usersdb.ToList();


            List<Apartment> lista = new List<Apartment>();

            foreach (Apartment ap in dal.apartmentsdb.ToDictionary(x => x.ApartmentId, x => x).Values)
            {
                foreach (Photo ph in dal.photosdb.ToDictionary(x => x.PhotoId, x => x).Values)
                {
                    if (ap.ApartmentId == ph.ApartmentId)
                    {
                        ap.images.Add(ph);
                    }
                }

                if (ap.UserId == sc.UserId)
                {
                    lista.Add(ap);
                }

            }


            sc.apartments = lista;

            if (login)
            {
                return Redirect(Url.Content("~/"));

            }
            else
                return Redirect(Url.Content("~/"));

        }

        public ActionResult FailedLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminChangeRole(User user, string role)
        {

            dal.usersdb.ToDictionary(x => x.username, x => x)[user.username].role = user.role;
            dal.SaveChanges();
            TempData["notice"] = "Successfully changed";

            return RedirectToAction("AdminViewUsers");
        }

        public ActionResult UserChangeData()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ChangeProfileData(User u)
        {
            User sc = (User)Session["user"];

            dal.usersdb.ToDictionary(x => x.username, x => x)[sc.username].name = u.name;
            dal.usersdb.ToDictionary(x => x.username, x => x)[sc.username].surname = u.surname;
            dal.usersdb.ToDictionary(x => x.username, x => x)[sc.username].email = u.email;
            dal.usersdb.ToDictionary(x => x.username, x => x)[sc.username].password = u.password;
            dal.usersdb.ToDictionary(x => x.username, x => x)[sc.username].sex = u.sex;
            dal.usersdb.ToDictionary(x => x.username, x => x)[sc.username].username = u.username;

            dal.SaveChanges();

            User temp = dal.usersdb.Where(x => x.UserId == sc.UserId).Select(x => x).Single();
            temp.apartments = dal.apartmentsdb.Where(x => x.UserId == sc.UserId).Select(x => x).ToList();
            foreach (Apartment ap in temp.apartments)
            {          
                ap.images = dal.photosdb.Where(x => x.ApartmentId == ap.ApartmentId).Select(x => x).ToList();
                ap.amenities = dal.amenitiesdb.Where(x => x.ApartmentId == ap.ApartmentId).Select(x => x).ToList();
            }

            Session["user"] = temp;

            return Redirect(Url.Content("~/"));
        }

        public ActionResult LogOut()
        {
            Session.Remove("user");
            return Redirect(Url.Content("~/"));

        } 
    }
}