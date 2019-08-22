using System;
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

        public string str;

        [HttpPost]
        public ActionResult Index(string selectedValue)
        {
            List<Apartment> lista = new List<Apartment>();

            Session["dropdown"] = selectedValue;


            return Json(lista);
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

            List<Apartment> lista = new List<Apartment>();


            foreach (Apartment ap in dal.apartmentsdb.ToDictionary(x => x.ApartmentId, x => x).Values)
            {
                Apartment apartment = new Apartment();
                apartment = ap;
                apartment.images = new List<Photo>();

                foreach (Photo ph in dal.photosdb.ToDictionary(x => x.PhotoId, x => x).Values)
                {
                    Photo photo = new Photo();
                    photo = ph;
                    if (apartment.ApartmentId == photo.ApartmentId)
                    {
                        apartment.images.Add(photo);
                    }
                }

                lista.Add(apartment);


            }


            if ((string)Session["dropdown"] == "sortasc")
            {
                ViewBag.lista = lista.OrderBy(x => x.price_per_night).ToList();
            }
            else if ((string)Session["dropdown"] == "sortdesc")
            {
                ViewBag.lista = lista.OrderByDescending(x => x.price_per_night).ToList();

            }
            else
            {
                ViewBag.lista = lista;

            }

            return View();
        }

    }
}