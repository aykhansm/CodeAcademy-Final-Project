using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }
        public static readonly ILoggerFactory loggerFactory = new LoggerFactory();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
                .EnableSensitiveDataLogging()
                .UseSqlServer(Configuration.GetConnectionString("Default"));
        }
        public IConfiguration Configuration { get; }
       
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyTag> PropertyTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
    }
}
