﻿using FlightFinder.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Data {
    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        //public DbSet<Flight> Flight { get; set; }
    }
}
