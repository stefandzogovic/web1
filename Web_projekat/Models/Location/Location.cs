using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Address address { get; set; }

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}