using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ahuvi.Model
{
    public class AhuviContext : DbContext
    {

        public AhuviContext(DbContextOptions<AhuviContext> options) : base(options)
        {
        }
        public DbSet<Insured> Insured { get; set; }
        public DbSet<Immunization> Immunization { get; set; }
        public DbSet<Disease> Disease { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {



        }
    }
}
