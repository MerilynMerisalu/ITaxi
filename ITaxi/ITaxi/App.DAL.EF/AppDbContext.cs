using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<LangStr> LangStrings { get; set; } = default!;
    public DbSet<Translation> Translations { get; set; } = default!;
    public DbSet<Country> Countries { get; set; } = default!;
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

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var relationship in builder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        builder.Entity<Drive>().HasOne(x => x.Comment).WithOne(x => x.Drive).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Booking>().HasOne(x => x.Drive).WithOne(x => x.Booking).OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<DriverAndDriverLicenseCategory>()
            .HasOne(e => e.Driver)
            .WithMany(l => l.DriverLicenseCategories)
            .HasForeignKey(l => l.DriverId);
        builder.Entity<DriverAndDriverLicenseCategory>()
            .HasOne(e => e.DriverLicenseCategory)
            .WithMany(d => d.Drivers)
            .HasForeignKey(d => d.DriverLicenseCategoryId);
    }

    // only needed if postgres db is used
    /*public override int SaveChanges()
    {
        FixEntities(this);
        
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        FixEntities(this);
        
        return base.SaveChangesAsync(cancellationToken);
    }

    private void FixEntities(AppDbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);


        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }
        

    } */
}