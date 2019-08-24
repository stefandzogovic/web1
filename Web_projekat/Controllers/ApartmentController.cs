using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_projekat.Models;
using Web_projekat.Models.User;

namespace Web_projekat.Controllers
{
    public class ApartmentController : Controller
    {

        DataAccessLayer dal = new DataAccessLayer();

        #region Convert HttpPostedFileBase to byte array
        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }
        #endregion

        // GET: Apartment
        public ActionResult HostViewApartments()
        {
            return View();
        }

        public ActionResult HostAddApartment()
        {
            return View();
        }


        #region Add Apartment
        [HttpPost]
        public ActionResult AddApartment(Apartment apartment, Dictionary<string, string> list, List<HttpPostedFileBase> imageUpload, List<string> amenitybasic,
            List<string> amenityfamily, List<string> amenityfacility, List<string> amenitydining)
        {
            User sc = (User)Session["user"];

            Apartment ap = new Apartment();
            ap.images = new List<Photo>();
            ap.amenities = new List<Amenity>();
            ap.User = dal.usersdb.FirstOrDefault(g => g.username == sc.username);
            ap.UserId = dal.usersdb.FirstOrDefault(g => g.username == sc.username).UserId;
            ap.type = Models.Type.Apartment;
            ap.number_of_guests = apartment.number_of_guests;
            ap.number_of_rooms = apartment.number_of_rooms;
            ap.price_per_night = apartment.price_per_night;
            foreach (HttpPostedFileBase image in imageUpload)
            {
                Photo photo = new Photo();
                photo.ApartmentId = ap.ApartmentId;
                photo.Apartment = ap;
                photo.Description = image.FileName;
                photo.ImageBytes = ConvertToByte(image);
                ap.images.Add(photo);
            }

            foreach(string basic in amenitybasic)
            {
                Amenity amenity = new Amenity();
                amenity.Apartment = ap;
                amenity.ApartmentId = ap.ApartmentId;
                amenity.name = basic;
                amenity.type = 1;
                ap.amenities.Add(amenity);
            }
            foreach (string basic in amenityfamily)
            {
                Amenity amenity = new Amenity();
                amenity.Apartment = ap;
                amenity.ApartmentId = ap.ApartmentId;
                amenity.name = basic;
                amenity.type = 2;
                ap.amenities.Add(amenity);
            }
            foreach (string basic in amenityfacility)
            {
                Amenity amenity = new Amenity();
                amenity.Apartment = ap;
                amenity.ApartmentId = ap.ApartmentId;
                amenity.name = basic;
                amenity.type = 3;
                ap.amenities.Add(amenity);
            }
            foreach (string basic in amenitydining)
            {
                Amenity amenity = new Amenity();
                amenity.Apartment = ap;
                amenity.ApartmentId = ap.ApartmentId;
                amenity.name = basic;
                amenity.type = 4;
                ap.amenities.Add(amenity);
            }

            sc.apartments.Add(ap);
            dal.apartmentsdb.Add(ap);
            try
            {
                dal.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return RedirectToAction("HostViewApartments");
        }
        #endregion

        public ActionResult AdminInactiveApartments()
        {
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

                }

                foreach (Photo ph in dal.photosdb.ToDictionary(x => x.PhotoId, x => x).Values)
                {
                    Photo photo = new Photo();
                    photo = ph;
                }

                lista.Add(user);
            }

            ViewBag.lista = lista;

            return View();
        }

        [HttpPost]
        public ActionResult AdminInactiveApartments(string active, int id)
        {

            User sc = (User)Session["user"];

            if (active == "Active")
                dal.apartmentsdb.ToDictionary(x => x.ApartmentId, x => x)[id].active = true;
            else
            {
                dal.apartmentsdb.ToDictionary(x => x.ApartmentId, x => x)[id].active = false;

            }

            dal.SaveChanges();


            Session["user"] = sc;

            return RedirectToAction("Index", "Home");
        }

    }

}