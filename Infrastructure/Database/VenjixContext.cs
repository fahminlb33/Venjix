using Microsoft.EntityFrameworkCore;
using Venjix.Infrastructure.Authentication;
using Venjix.Models.Entities;

namespace Venjix.Infrastructure.Database
{
    public class VenjixContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Webhook> Webhooks { get; set; }
        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<Recording> Recordings { get; set; }

        public VenjixContext(DbContextOptions<VenjixContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    UserId = Roles.SuperadminUserId,
                    Role = Roles.Admin,
                    Username = "admin",
                    Password = "$2y$12$XIMeV8tAOoC7D0XRJF5TjOQNy0T9Wj71JkETAdEmrjH6X9nIf50ZO"
                });

            modelBuilder.Entity<Trigger>()
                .HasOne(x => x.Webhook)
                .WithMany(x => x.Triggers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.WebhookId)
                .IsRequired(false);

            modelBuilder.Entity<Recording>()
                .Property(x => x.Value)
                .HasPrecision(4);
        }
    }
}