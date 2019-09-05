using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class AvailableAmenities
    {
        public int AvailableAmenitiesId { get; set; }
        public int type { get; set; }
        public string name { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}