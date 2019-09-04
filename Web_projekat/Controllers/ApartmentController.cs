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
        public static List<string> basic = new List<string> { "Wifi", "Laptop_Friendly_Workspace", "Cable_TV", "Washer", "Air_Conditioning", "TV", "Heating" };
        public static List<string> family = new List<string> { "Crib", "High_Chair", "Travel_Crib", "Room-darkening_Shades", "Window_Guards" };
        public static List<string> facility = new List<string> { "Elevator", "Paid_Parking_Off_Premices", "Single_Level_Home_(No_Stairs)", "Free_Street_Parking" };
        public static List<string> dining = new List<string> { "Kitchen", "Coffee_Maker", "Cooking_Basics(Pots,_Pans,_Salt_Pepper", "Dishes_and_Silverware", "Microwave", "Refrigerator" };

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
            ViewBag.basic = basic;
            ViewBag.family = family;
            ViewBag.facility = facility;
            ViewBag.dining = dining;
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

        [HttpPost]
        public ActionResult ChangeApartment(int apartmentid, int userid)
        {
            ViewBag.basic = basic;
            ViewBag.family = family;
            ViewBag.facility = facility;
            ViewBag.dining = dining;

            User u = dal.usersdb.ToDictionary(x => x.UserId, x => x)[userid];
            Apartment ap = dal.apartmentsdb.ToDictionary(x => x.ApartmentId, x => x)[apartmentid];
            ap.images = new List<Photo>();
            ap.amenities = new List<Amenity>();

            ap.amenities = dal.amenitiesdb.Where(x => x.ApartmentId == ap.ApartmentId).Where(x => x.IsDeleted == false).Select(x => x).ToList();
            ap.images = dal.photosdb.Where(x => x.ApartmentId == ap.ApartmentId).Select(x => x).ToList();

            ViewBag.ap = ap;

            return View();
        }

        [HttpPost]
        public ActionResult Change(Apartment apartment, Dictionary<string, string> list, List<HttpPostedFileBase> imageUpload, List<string> amenitybasic,
            List<string> amenityfamily, List<string> amenityfacility, List<string> amenitydining, int apartmentId, List<string> imageUpload1)
        {
            User sc = (User)Session["user"];

            sc.apartments = dal.apartmentsdb.Where(x => x.UserId == sc.UserId).Select(x => x).ToList();

            foreach (Apartment temp in sc.apartments)
            {
                temp.images = dal.photosdb.Where(x => x.ApartmentId == temp.ApartmentId).Select(x => x).ToList();
                temp.amenities = dal.amenitiesdb.Where(x => x.ApartmentId == temp.ApartmentId).Select(x => x).ToList();

                if (temp.ApartmentId == apartment.ApartmentId)
                {
                    temp.number_of_guests = apartment.number_of_guests;

                    temp.number_of_rooms = apartment.number_of_rooms;

                    temp.price_per_night = apartment.price_per_night;
                }

                if (amenitybasic != null)
                {
                    foreach (Amenity am in temp.amenities.Where(x => x.type == 1).Where(x =>x.IsDeleted == false).Select(x => x))
                    {
                        if (!amenitybasic.Contains(am.name))
                        {
                            am.IsDeleted = true;
                        }
                    }

                    foreach (string str in amenitybasic)
                    {
                        List<string> templist = temp.amenities.Where(x =>x.IsDeleted == false).Select(x => x.name).ToList();
                        if (!templist.Contains(str))
                        {
                            Amenity am = new Amenity();
                            am.Apartment = temp;
                            am.ApartmentId = temp.ApartmentId;
                            am.name = str;
                            am.type = 1;
                            temp.amenities.Add(am);
                        }
                    }
                }
                else
                {
                    foreach (Amenity am in temp.amenities.Where(x => x.type == 1).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        am.IsDeleted = true;
                    }
                }

                if (amenityfamily != null)
                {

                    foreach (Amenity am in temp.amenities.Where(x => x.type == 2).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        if (!amenityfamily.Contains(am.name))
                        {
                            am.IsDeleted = true;
                        }
                    }

                    foreach (string str in amenityfamily)
                    {
                        List<string> templist = temp.amenities.Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                        if (!templist.Contains(str))
                        {
                            Amenity am = new Amenity();
                            am.Apartment = temp;
                            am.ApartmentId = temp.ApartmentId;
                            am.name = str;
                            am.type = 2;
                            temp.amenities.Add(am);
                        }
                    }
                }
                else
                {
                    foreach (Amenity am in temp.amenities.Where(x => x.type == 2).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        am.IsDeleted = true;
                    }
                }

                if (amenityfacility != null)
                {

                    foreach (Amenity am in temp.amenities.Where(x => x.type == 3).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        if (!amenityfacility.Contains(am.name))
                        {
                            am.IsDeleted = true;
                        }
                    }

                    foreach (string str in amenityfacility)
                    {
                        List<string> templist = temp.amenities.Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                        if (!templist.Contains(str))
                        {
                            Amenity am = new Amenity();
                            am.Apartment = temp;
                            am.ApartmentId = temp.ApartmentId;
                            am.name = str;
                            am.type = 3;
                            temp.amenities.Add(am);
                        }
                    }
                }
                else
                {
                    foreach (Amenity am in temp.amenities.Where(x => x.type == 3).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        am.IsDeleted = true;
                    }
                }

                if (amenitydining != null)
                {

                    foreach (Amenity am in temp.amenities.Where(x => x.type == 4).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        if (!amenitydining.Contains(am.name))
                        {
                            am.IsDeleted = true;
                        }
                    }

                    foreach (string str in amenitydining)
                    {
                        List<string> templist = temp.amenities.Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                        if (!templist.Contains(str))
                        {
                            Amenity am = new Amenity();
                            am.Apartment = temp;
                            am.ApartmentId = temp.ApartmentId;
                            am.name = str;
                            am.type = 4;
                            temp.amenities.Add(am);
                        }
                    }
                }
                else
                {
                    foreach (Amenity am in temp.amenities.Where(x => x.type == 4).Where(x => x.IsDeleted == false).Select(x => x))
                    {
                        am.IsDeleted = true;
                    }
                }

                if (imageUpload[0] != null)
                {
                    temp.images.Clear();

                    foreach (HttpPostedFileBase image in imageUpload)
                    {
                        Photo photo = new Photo();
                        photo.ApartmentId = temp.ApartmentId;
                        photo.Apartment = temp;
                        photo.Description = image.FileName;
                        photo.ImageBytes = ConvertToByte(image);
                        temp.images.Add(photo);
                    }
                }
            }




            sc = sc;

                dal.SaveChanges();



                return View("HostViewApartments");
            
        }
    }

}