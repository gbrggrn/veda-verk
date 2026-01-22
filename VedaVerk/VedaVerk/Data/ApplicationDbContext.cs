using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VedaVerk.Models;

namespace VedaVerk.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<VegBag> VegBags { get; set; }
	}
}
