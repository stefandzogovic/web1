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
            ViewBag.basic = dal.availableamenitiesdb.Where(x => x.type == 1).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            ViewBag.family = dal.availableamenitiesdb.Where(x => x.type == 2).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            ViewBag.facility = dal.availableamenitiesdb.Where(x => x.type == 3).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            ViewBag.dining = dal.availableamenitiesdb.Where(x => x.type == 4).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            return View();
        }


        #region Add Apartment
        [HttpPost]
        public ActionResult AddApartment(Apartment apartment, Dictionary<string, string> list, List<HttpPostedFileBase> imageUpload, List<string> amenitybasic,
            List<string> amenityfamily, List<string> amenityfacility, string tajp ,List<string> amenitydining, Location location, Address address)
        {
            User sc = (User)Session["user"];

            Apartment ap = new Apartment();
            ap.images = new List<Photo>();
            Location loc = new Location();
            Address ad = new Address();
            ap.amenities = new List<Amenity>();
            if (tajp == "room")
            {
                ap.type = Models.Type.Room;
            }
            else
            {
                ap.type = Models.Type.Apartment;

            }
            ap.User = dal.usersdb.FirstOrDefault(g => g.username == sc.username);
            ap.UserId = dal.usersdb.FirstOrDefault(g => g.username == sc.username).UserId;
            ap.number_of_guests = apartment.number_of_guests;
            ap.Times = new DateTimeCollection();
            ap.number_of_rooms = apartment.number_of_rooms;
            ap.price_per_night = apartment.price_per_night;

            loc.Apartment = ap;
            loc.latitude = location.latitude;
            loc.longitude = location.longitude;


            ad.Location = loc;
            ad.number = address.number;
            ad.street = address.street;
            ad.postal_code = address.postal_code;
            ad.city = address.city;

            loc.Address = ad;

            ap.Location = loc;

            

            List<string> templista = new List<string>();
            foreach(string str in list.Keys)
            {
                templista.Add(str);
            }
            ap.Times.AddRange(templista);
            ap.Times = ap.Times;
            if (imageUpload[0] != null)
            {
                foreach (HttpPostedFileBase image in imageUpload)
                {
                    Photo photo = new Photo();
                    photo.ApartmentId = ap.ApartmentId;
                    photo.Apartment = ap;
                    photo.Description = image.FileName;
                    photo.ImageBytes = ConvertToByte(image);
                    ap.images.Add(photo);
                }
            }
            if (amenitybasic != null)
            {
                foreach (string basic in amenitybasic)
                {
                    Amenity amenity = new Amenity();
                    amenity.Apartment = ap;
                    amenity.ApartmentId = ap.ApartmentId;
                    amenity.name = basic;
                    amenity.type = 1;
                    ap.amenities.Add(amenity);
                }
            }
            if (amenityfamily != null)
            {
                foreach (string basic in amenityfamily)
                {
                    Amenity amenity = new Amenity();
                    amenity.Apartment = ap;
                    amenity.ApartmentId = ap.ApartmentId;
                    amenity.name = basic;
                    amenity.type = 2;
                    ap.amenities.Add(amenity);
                }
            }
            if (amenityfacility != null)
            {
                foreach (string basic in amenityfacility)
                {
                    Amenity amenity = new Amenity();
                    amenity.Apartment = ap;
                    amenity.ApartmentId = ap.ApartmentId;
                    amenity.name = basic;
                    amenity.type = 3;
                    ap.amenities.Add(amenity);
                }
            }
            if (amenitydining != null)
            {
                foreach (string basic in amenitydining)
                {
                    Amenity amenity = new Amenity();
                    amenity.Apartment = ap;
                    amenity.ApartmentId = ap.ApartmentId;
                    amenity.name = basic;
                    amenity.type = 4;
                    ap.amenities.Add(amenity);
                }
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
                u.apartments = dal.apartmentsdb.Select(x => x).Where(x => x.UserId == u.UserId).ToList();

                foreach(Apartment ap in u.apartments)
                {
                    ap.images = dal.photosdb.Select(x => x).Where(x => x.ApartmentId == ap.ApartmentId).ToList();
                    ap.amenities = dal.amenitiesdb.Select(x => x).Where(x => x.ApartmentId == ap.ApartmentId).ToList();
                }



                lista.Add(u);
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
            ViewBag.basic = dal.availableamenitiesdb.Where(x => x.type == 1).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            ViewBag.family = dal.availableamenitiesdb.Where(x => x.type == 2).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            ViewBag.facility = dal.availableamenitiesdb.Where(x => x.type == 3).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
            ViewBag.dining = dal.availableamenitiesdb.Where(x => x.type == 4).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();

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
            List<string> amenityfamily, string tajp, List<string> amenityfacility, List<string> amenitydining, int apartmentId, List<string> imageUpload1, Location location, Address address)
        {
            User sc = (User)Session["user"];

            sc.apartments = dal.apartmentsdb.Where(x => x.UserId == sc.UserId).Select(x => x).ToList();

            foreach (Apartment temp in sc.apartments)
            {
                temp.images = dal.photosdb.Where(x => x.ApartmentId == temp.ApartmentId).Select(x => x).ToList();
                temp.amenities = dal.amenitiesdb.Where(x => x.ApartmentId == temp.ApartmentId).Select(x => x).ToList();
                List<string> dates = new List<string>();
  

                if (temp.ApartmentId == apartment.ApartmentId)
                 {
                    if (tajp == "room")
                    {
                        temp.type = Models.Type.Room;
                    }
                    else
                    {
                        temp.type = Models.Type.Apartment;

                    }

                    foreach (KeyValuePair<string, string> tempdates in list)
                    {

                        dates.Add(tempdates.Key);
                        dates.Add(tempdates.Value);
                    }

                    temp.Times.Clear();
                    temp.Times.AddRange(dates);
                    temp.number_of_guests = apartment.number_of_guests;

                    temp.number_of_rooms = apartment.number_of_rooms;

                    temp.price_per_night = apartment.price_per_night;

                    temp.Location.latitude = location.latitude;
                    temp.Location.longitude = location.longitude;
                    temp.Location.Address.city = address.city;
                    temp.Location.Address.number = address.number;
                    temp.Location.Address.postal_code = address.postal_code;
                    temp.Location.Address.street = address.street;
           
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
                    foreach(Photo im in temp.images)
                    {
                        im.IsDeleted = true;
                    }

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


            dal.SaveChanges();



            return RedirectToAction("HostViewApartments");


        }

        public ActionResult AdminChangeAmenities()
        {
            ViewBag.lista = dal.availableamenitiesdb.Where(x => x.IsDeleted == false).Select(x => x).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AdminChangeAmenities(List<string> amenitybasic,
            List<string> amenityfamily, List<string> amenityfacility, List<string> amenitydining)
        {
            if (amenitybasic != null)
            {
                foreach (AvailableAmenities am in dal.availableamenitiesdb.Where(x => x.type == 1).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                {
                    if (!amenitybasic.Contains(am.name))
                    {
                        am.IsDeleted = true;
                        foreach (Amenity ame in dal.amenitiesdb.Where(x => x.name == am.name).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                        {
                            ame.IsDeleted = true;
                        }
                    }
                }

                foreach (string str in amenitybasic)
                {
                    List<string> templist = dal.availableamenitiesdb.Where(x => x.type == 1).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                    if (!templist.Contains(str) && str != "") //and if string != ""
                    {
                        AvailableAmenities am = new AvailableAmenities();
                        am.name = str.Replace(' ', '\xa0');
                        am.type = 1;
                        dal.availableamenitiesdb.Add(am);

                    }
                }
            }

            if (amenityfamily != null)
            {
                foreach (AvailableAmenities am in dal.availableamenitiesdb.Where(x => x.type == 2).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                {
                    if (!amenityfamily.Contains(am.name))
                    {
                        am.IsDeleted = true;
                        foreach (Amenity ame in dal.amenitiesdb.Where(x => x.name == am.name).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                        {
                            ame.IsDeleted = true;
                        }
                    }
                }

                foreach (string str in amenityfamily)
                {
                    List<string> templist = dal.availableamenitiesdb.Where(x => x.type == 2).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                    if (!templist.Contains(str) && str != "")
                    {
                        AvailableAmenities am = new AvailableAmenities();
                        am.name = str.Replace(' ', '\xa0');
                        am.type = 2;
                        dal.availableamenitiesdb.Add(am);
                    }
                }
            }
            if (amenityfacility != null)
            {
                foreach (AvailableAmenities am in dal.availableamenitiesdb.Where(x => x.type == 3).Where(x => x.IsDeleted == false).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                {
                    if (!amenityfacility.Contains(am.name))
                    {
                        am.IsDeleted = true;
                        foreach (Amenity ame in dal.amenitiesdb.Where(x => x.name == am.name).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                        {
                            ame.IsDeleted = true;
                        }
                    }
                }

                foreach (string str in amenityfacility)
                {
                    List<string> templist = dal.availableamenitiesdb.Where(x => x.type == 3).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                    if (!templist.Contains(str) && str != "")
                    {
                        AvailableAmenities am = new AvailableAmenities();
                        am.name = str.Replace(' ', '\xa0');
                        am.type = 3;
                        dal.availableamenitiesdb.Add(am);

                    }
                }
            }
            if (amenitydining != null)
            {
                foreach (AvailableAmenities am in dal.availableamenitiesdb.Where(x => x.type == 4).Where(x => x.IsDeleted == false).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                {
                    if (!amenitydining.Contains(am.name))
                    {
                        am.IsDeleted = true;
                        foreach (Amenity ame in dal.amenitiesdb.Where(x => x.name == am.name).Where(x => x.IsDeleted == false).Select(x => x).ToList())
                        {
                            ame.IsDeleted = true;
                        }
                    }
                }

                foreach (string str in amenitydining)
                {
                    List<string> templist = dal.availableamenitiesdb.Where(x => x.type == 4).Where(x => x.IsDeleted == false).Select(x => x.name).ToList();
                    if (!templist.Contains(str) && str != "")
                    {
                        AvailableAmenities am = new AvailableAmenities();
                        am.name = str.Replace(' ', '\xa0');
                        am.type = 4;
                        dal.availableamenitiesdb.Add(am);
                    }
                }
            }

            dal.SaveChanges();
            ViewBag.lista = dal.availableamenitiesdb.Where(x => x.IsDeleted == false).Select(x => x).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AdminDeleteApartment(int id)
        {
            Apartment ap = dal.apartmentsdb.Select(x => x).Where(x => x.ApartmentId == id).Single();
            ap.IsDeleted = true;
            List<Photo> photo = dal.photosdb.Select(x => x).Where(x => x.ApartmentId == id).ToList();
            foreach(Photo ph in photo)
            {
                ph.IsDeleted = true;
            }
            List<Amenity> amenity = dal.amenitiesdb.Select(x => x).Where(x => x.ApartmentId == id).ToList();
            foreach(Amenity am in amenity)
            {
                am.IsDeleted = true;
            }

            Location loc = dal.locationsdb.Select(x => x).Where(x => x.Apartment.ApartmentId == id).Single();
            loc.IsDeleted = true;
            Address addr = dal.addresses.Select(x => x).Where(x => x.Location.LocationId == loc.LocationId).Single();
            addr.IsDeleted = true;
            dal.SaveChanges();
            return RedirectToAction("AdminInactiveApartments");
        }
        
        [HttpPost]
        public ActionResult ViewApartment(int apartmentid)
        {
            return View();
        }




    }

}