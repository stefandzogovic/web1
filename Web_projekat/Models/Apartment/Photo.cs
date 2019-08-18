using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        public int ApartmentId { get; set; }

        public Apartment Apartment { get; set; }

    }
}