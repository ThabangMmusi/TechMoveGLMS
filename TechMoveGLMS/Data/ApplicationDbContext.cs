using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverSchedule> DriverSchedules { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Driver configuration
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.VehicleNumber).HasMaxLength(20);
            });

            // DriverSchedule configuration
            modelBuilder.Entity<DriverSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DriverName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DriverPhone).HasMaxLength(20);
                entity.Property(e => e.VehicleNumber).HasMaxLength(20);
                entity.Property(e => e.PickupLocation).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DeliveryLocation).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Administrator",
                    Email = "admin@techmove.com",
                    Password = "Admin123",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

        }
            
        }
    }
