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
           

            Admins admini = new Admins("~/App_Data/admini.txt");
            HttpContext.Current.Application["admini"] = admini;
        }
    }
}
