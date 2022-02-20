using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    
    public DbSet<County> Counties { get; set; } = default!;
    public DbSet<City> Cities { get; set; } = default!;
    public DbSet<Photo> Photos { get; set; } = default!;
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<DriverLicenseCategory> DriverLicenseCategories { get; set; } = default!;
    public DbSet<DriverAndDriverLicenseCategory> DriverAndDriverLicenseCategories { get; set; } = default!;
    public DbSet<Driver> Drivers { get; set; } = default!;
    public DbSet<VehicleMark> VehicleMarks { get; set; } = default!;
    public DbSet<VehicleModel> VehicleModels { get; set; } = default!;
    public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
    public DbSet<Schedule> Schedules { get; set; } = default!;
    public DbSet<RideTime> RideTimes { get; set; } = default!;
    public DbSet<DisabilityType> DisabilityTypes { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Booking> Bookings { get; set; } = default!;
    public DbSet<Drive> Drives { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var relationship in builder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        builder.Entity<DriverAndDriverLicenseCategory>()
            .HasOne(e => e.Driver)
            .WithMany(l => l.DriverLicenseCategories)
            .HasForeignKey(l => l.DriverId);
        builder.Entity<DriverAndDriverLicenseCategory>()
            .HasOne(e => e.DriverLicenseCategory)
            .WithMany(d => d.Drivers)
            .HasForeignKey(d => d.DriverLicenseCategoryId);
    }
}