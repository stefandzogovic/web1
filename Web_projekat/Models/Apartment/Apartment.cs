using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_projekat.Models
{
    public class Apartment
    {
        public int ApartmentId { get; set; }
        public Type type { get; set; }
        public int number_of_rooms { get; set; }
        public int number_of_guests { get; set; }
        // public Location location { get; set; }
        public bool active { get; set; }
        public double price_per_night { get; set; }
        //public List<string> comments { get; set; }
        public List<Photo> images { get; set; }
        public List<Amenity> amenities { get; set; }

        public User.User User { get; set; }
        public int UserId { get; set; }

    }
}