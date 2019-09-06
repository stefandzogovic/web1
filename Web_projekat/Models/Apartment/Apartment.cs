using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public virtual Location Location { get; set; }
        public bool active { get; set; }
        public double price_per_night { get; set; }
        public List<Photo> images { get; set; }
        public List<Amenity> amenities { get; set; }
        public DateTimeCollection Times { get; set; }
        
        public bool IsDeleted { get; set; } = false;

        public User.User User { get; set; }
        public int UserId { get; set; }

    }

    [ComplexType]
    public class DateTimeCollection : Collection<string>
    {
        public void AddRange(IEnumerable<string> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        [Column("Times")]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Clear();
                    return;
                }

                var items = JsonConvert.DeserializeObject<string[]>(value);
                Clear();
                AddRange(items);
            }
        }
    }
}