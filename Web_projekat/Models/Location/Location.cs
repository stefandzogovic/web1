using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Location
    {

        public int LocationId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public virtual Address Address { get; set; }

        public virtual Apartment Apartment { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}