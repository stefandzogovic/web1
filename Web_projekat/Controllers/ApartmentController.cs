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

        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }

        // GET: Apartment
        public ActionResult HostViewApartments()
        {
            return View();
        }

        public ActionResult HostAddApartment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddApartment(Apartment apartment, Dictionary<string, string> list, List<HttpPostedFileBase> imageUpload)
        {
            User sc = (User)Session["user"];
          
            Apartment ap = new Apartment();
            ap.images = new List<Photo>();
            ap.User = dal.usersdb.FirstOrDefault(g => g.username == sc.username);
            ap.UserId = dal.usersdb.FirstOrDefault(g => g.username == sc.username).UserId;
            ap.type = Models.Type.Apartment;
            ap.number_of_guests = apartment.number_of_guests;
            ap.number_of_rooms = apartment.number_of_rooms;
            ap.price_per_night = apartment.price_per_night;
            foreach(HttpPostedFileBase image in imageUpload)
            {
                Photo photo = new Photo();
                photo.ApartmentId = ap.ApartmentId;
                photo.Apartment = ap;
                photo.Description = image.FileName;
                photo.ImageBytes = ConvertToByte(image);
                ap.images.Add(photo);
            }


            dal.apartmentsdb.Add(ap);
            dal.SaveChanges();

            return RedirectToAction("HostViewApartments");
        }
    }
}