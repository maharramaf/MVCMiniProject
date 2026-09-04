
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
        public  DbSet<Video> Videos { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<CourseInfo> CourseInfos { get; set; }
        public DbSet<CourseImage> CourseImages { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<PlatformAbout> PlatformAbouts { get; set; }
        public DbSet<VisionAbout> VisionAbouts { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.Email).HasMaxLength(256);
                entity.Property(u => u.Username).HasMaxLength(256);
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
