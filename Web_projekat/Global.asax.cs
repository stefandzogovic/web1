using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web_projekat.Models.User;

namespace Web_projekat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DataAccessLayer dal = new DataAccessLayer();
            dal.Database.CreateIfNotExists();

            List<string> basic = new List<string> { "Wifi", "Laptop_Friendly_Workspace", "Cable_TV", "Washer", "Air_Conditioning", "TV", "Heating" };
            List<string> family = new List<string> { "Crib", "High_Chair", "Travel_Crib", "Room-darkening_Shades", "Window_Guards" };
            List<string> facility = new List<string> { "Elevator", "Paid_Parking_Off_Premices", "Single_Level_Home_(No_Stairs)", "Free_Street_Parking" };
            List<string> dining = new List<string> { "Kitchen", "Coffee_Maker", "Cooking_Basics(Pots,_Pans,_Salt_Pepper", "Dishes_and_Silverware", "Microwave", "Refrigerator" };

            if (dal.availableamenitiesdb.ToList().Count == 0)
            {
                 foreach(string str in basic)
                 {
                     dal.availableamenitiesdb.Add(new Models.AvailableAmenities { type = 1, name = str.Replace('_', ' ')});
                 }
                 foreach (string str in family)
                 {

                     dal.availableamenitiesdb.Add(new Models.AvailableAmenities { type = 2, name = str.Replace('_', ' ') });
                 }
                 foreach (string str in facility)
                 {

                         dal.availableamenitiesdb.Add(new Models.AvailableAmenities { type = 3, name = str.Replace('_', ' ') });
                 }
                 foreach (string str in dining)
                 {

                         dal.availableamenitiesdb.Add(new Models.AvailableAmenities { type = 4, name = str.Replace('_', ' ') });
                 }


                 dal.SaveChanges();
                 
            }

            Admins admini = new Admins("~/App_Data/admini.txt");
            HttpContext.Current.Application["admini"] = admini;
        }
    }
}
