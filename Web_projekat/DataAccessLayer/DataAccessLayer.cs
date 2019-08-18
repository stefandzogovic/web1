using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web_projekat.Models;
using Web_projekat.Models.User;

namespace Web_projekat
{
    public class DataAccessLayer  :  DbContext
    {
        public DataAccessLayer() : base("ConnectionString")
        {

        }

        public DbSet<User> usersdb { get; set; }
        public DbSet<Apartment> apartmentsdb { get; set; }
        public DbSet<Photo> photosdb { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(c => c.apartments).WithRequired(m => m.User);
            modelBuilder.Entity<Apartment>().HasMany(x => x.images).WithRequired(y => y.Apartment);
        }
    }
}