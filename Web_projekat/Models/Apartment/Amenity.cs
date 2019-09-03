using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Amenity
    {
        public int AmenityId { get; set; }
        public int type { get; set; }
        public string name { get; set; }

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}