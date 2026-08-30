
using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Models;

namespace MVCMiniProject.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Icon> Icons { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public  DbSet<Event> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Author> Authors { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
         

    }
}
