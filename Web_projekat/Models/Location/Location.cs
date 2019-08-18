using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Address address { get; set; }
    }
}