using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_projekat.Models.User
{
    public class User
    {
        public int UserId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public Sex sex { get; set; }
        public Role role { get; set; }
        public List<Apartment> apartments { get; set; }

        public bool IsDeleted { get; set; } = false;


    }
}