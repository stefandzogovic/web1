﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_projekat.Models;
using Web_projekat.Models.User;

namespace Web_projekat.Controllers
{
    public class HomeController : Controller
    {
        DataAccessLayer dal = new DataAccessLayer();

        [HttpPost]
        public ActionResult Index(string value)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            Admins admini = (Admins)HttpContext.Application["admini"];
            ViewBag.loggedin = false;
            User sc = (User)Session["user"];


            if (Session["user"] != null)
            {
                ViewBag.user = sc;
            }

            List<User> lista = new List<User>();


            foreach (User u in dal.usersdb.ToDictionary(x => x.username, x => x).Values)
            {
                User user = new User();
                user = u;
                user.apartments = new List<Apartment>();

                foreach (Apartment ap in dal.apartmentsdb.ToDictionary(x => x.ApartmentId, x => x).Values)
                {
                    Apartment apartment = new Apartment();
                    apartment = ap;
                    apartment.images = new List<Photo>();

                    foreach (Photo ph in dal.photosdb.ToDictionary(x => x.PhotoId, x => x).Values)
                    {
                        Photo photo = new Photo();
                        photo = ph;
                        if(apartment.ApartmentId == photo.ApartmentId)
                        {
                            apartment.images.Add(photo);
                        }
                    }


                }

                lista.Add(user);
            }

            ViewBag.lista = lista;
            return View();
        }
    }
}