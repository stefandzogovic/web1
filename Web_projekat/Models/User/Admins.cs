using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Web_projekat.Models.User
{
    public class Admins
    {
        public Dictionary<string, User> admini { get; set; }

        public Admins(string path)
        {
            path = HostingEnvironment.MapPath(path);
            admini = new Dictionary<string, User>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Sex sexx;
                switch(tokens[4])
                {
                    case "Male":
                        sexx = Sex.Male;
                        break;
                    case "Female":
                        sexx = Sex.Female;
                        break;
                    case "Other":
                        sexx = Sex.Other;
                        break;
                    default:
                        sexx = Sex.Other;
                        break;
                }
                User u = new User { name = tokens[0], surname = tokens[1], username = tokens[2], password = tokens[3], role = Role.Admin, sex =  sexx, email = tokens[5]};
                admini.Add(u.username, u);
            }
            sr.Close();
            stream.Close();
        }
    }
}