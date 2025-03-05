using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Infrastructure.Data
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SIPOC> SIPOCs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Notification>().HasKey(u => u.Id);
            modelBuilder.Entity<Customer>().HasKey(u => u.Id);
            modelBuilder.Entity<Supplier>().HasKey(u => u.Id);
            modelBuilder.Entity<SIPOC>().HasKey(u => u.Id);

            // One-to-many relationship between User and Role (each User has one Role)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Optionally specify delete behavior

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)  // Role can have multiple Users
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            // One-to-Many: SIPOC -> Supplier
            modelBuilder.Entity<SIPOC>()
                .HasOne(s => s.Supplier)
                .WithMany()
                .HasForeignKey(s => s.SupplierId);

            // One-to-Many: SIPOC -> Customer
            modelBuilder.Entity<SIPOC>()
                .HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId);
        }
    }
}
