using CleanArchMonolit.Domain.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchMonolit.Infrastruture.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SystemPermission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profiles>().HasData(
                new Profiles(1, "Admin"),
                new Profiles(2, "User")
            );
        }
    }
}
