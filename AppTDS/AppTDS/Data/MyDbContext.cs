using AppTDS.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTDS.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Contact> Contact { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.ContactId);
                entity.Property(e => e.Username).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.ContactList)
                      .HasForeignKey(e => e.UserId);
            });
        }
    }
}
