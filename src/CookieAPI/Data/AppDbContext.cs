using Microsoft.EntityFrameworkCore;
using CookieAPI.Entities;

namespace CookieAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)  : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(eb =>
            {
                eb.HasKey(e => e.Guid);
                eb.Property(e => e.Guid).ValueGeneratedOnAdd();
                eb.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                eb.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                eb.Property(e => e.EmailAddress).IsRequired().HasMaxLength(100);
                eb.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            });
        }
    }
}
