﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web_projekat.Models.User;

namespace Web_projekat
{
    public class DataAccessLayer  :  DbContext
    {
        public DataAccessLayer() : base("ConnectionString")
        {

        }

        public DbSet<User> usersdb { get; set; }
    }
}