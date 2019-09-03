using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string street { get; set; }
        public int number { get; set; }
        public string city { get; set; }
        public long postal_code { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}