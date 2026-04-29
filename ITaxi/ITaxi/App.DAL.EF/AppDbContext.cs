using App.Domain;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Contracts.Services;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Identity.Client;
using System.Collections;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
        _currentUserService = currentUserService;
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
    public DbSet<ExtraService> ExtraServices { get; set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var relationship in builder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;


        builder.Entity<ExtraService>(es => { es.Property(p => p.Price).HasPrecision(18, 2); });
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

        builder.Entity<County>()
                .HasIndex(c => new { c.CountryId, c.CountyEHAKCode })
                .IsUnique()
                .HasFilter("[CountyEHAKCode] IS NOT NULL");

        builder.Entity<Country>()
            .HasIndex(c => c.ISOCode)
            .IsUnique();

        
    }
    private void NormalizeCountyNames()
    {
        foreach (var entry in ChangeTracker.Entries<County>()
                     .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (!string.IsNullOrWhiteSpace(entry.Entity.CountyName))
            {
                entry.Entity.CountyName = entry.Entity.CountyName.Trim();
                entry.Entity.CountyNameNormalized = entry.Entity.CountyName.ToUpperInvariant();
            }
        }
    }

    
    public override int SaveChanges()
    {
        //FixEntities(this); Only needed if postgres db is used
        NormalizeCountyNames();
        ChangeMetadata();
       // ApplyIgnoredCascadeAsync()
        return base.SaveChanges();
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //FixEntities(this); Only needed if postgres db is used
        
        NormalizeCountyNames();
        ChangeMetadata();
        var result = await base.SaveChangesAsync(cancellationToken);

        await ApplyIgnoredCascadeAsync(cancellationToken);
        return result;

    }

    //private void FixEntities(AppDbContext context)
    //{
    //    var dateProperties = context.Model.GetEntityTypes()
    //        .SelectMany(t => t.GetProperties())
    //        .Where(p => p.ClrType == typeof(DateTime))
    //        .Select(z => new
    //        {
    //            ParentName = z.DeclaringEntityType.Name,
    //            PropertyName = z.Name
    //        });

    //    var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
    //        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
    //        .Select(x => x.Entity);


    //    foreach (var entity in editedEntitiesInTheDbContextGraph)
    //    {
    //        var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

    //        foreach (var property in entityFields)
    //        {
    //            var prop = entity.GetType().GetProperty(property.PropertyName);

    //            if (prop == null)
    //                continue;

    //            var originalValue = prop.GetValue(entity) as DateTime?;
    //            if (originalValue == null)
    //                continue;

    //            prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
    //        }
    //    }
        
    private void ChangeMetadata()
    {
        var currentUserEmail = _currentUserService.UserEmail ?? "System";
        var currentUtcTime = DateTime.UtcNow;
        foreach (var entity in ChangeTracker.Entries<IDomainEntityMeta>())
        {
            if (entity.State == EntityState.Modified)
            {
                entity.Entity.UpdatedBy = currentUserEmail;
                entity.Entity.UpdatedAt = currentUtcTime;
            }
            else if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedBy = currentUserEmail ?? "System";
                entity.Entity.CreatedAt = currentUtcTime;
                entity.Entity.UpdatedBy = currentUserEmail ?? "System";
                entity.Entity.UpdatedAt = currentUtcTime;
                
            }
        }
    }

    public async Task ApplyIgnoredCascadeAsync(CancellationToken cancellationToken)
    {
        var ignoredParentsByType = ChangeTracker.Entries()
            .Where(entry => entry.Entity is IDomainEntityMeta meta &&
            (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                && meta.IsIgnored)
            .Select(entry => new
            {
                Id = (Guid)entry.Property("Id").CurrentValue!,
                EntityType = entry.Metadata.ClrType
            })
            .GroupBy(x => x.EntityType).
                ToDictionary(x =>  x.Key,x => 
                    x.Select(x => x.Id).Distinct().ToList());
        if (ignoredParentsByType.Count == 0)
        {
            return;
        }

        foreach (var (parentType, parentIds) in ignoredParentsByType)
        {
            var foreignKeys = Model.GetEntityTypes().SelectMany(entityType => entityType.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType.ClrType == parentType)
                .Where(fk => fk.Properties.Count == 1)
                .Where(fk => typeof(IDomainEntityMeta).IsAssignableFrom(fk.DeclaringEntityType.ClrType))
                .Where(fk =>  !IsExcludedFromIgnoreCascade(fk.DeclaringEntityType.ClrType))
                .ToList();

            foreach (var foreignKey in foreignKeys)
            {
                var childType = foreignKey.DeclaringEntityType.ClrType;
                var foreignKeyPropertyName = foreignKey.Properties[0].Name;
                var method = typeof(AppDbContext)
                    .GetMethod(nameof(ExecuteIgnoreCascadeForChildTypeAsync),
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                    .MakeGenericMethod(childType);
                var rowsUpdated = await (Task<int>)method
                    .Invoke(this, new object[] { foreignKeyPropertyName, parentIds, cancellationToken });
            }
        }
    }

    private  Task<int> ExecuteIgnoreCascadeForChildTypeAsync<TChild>(string foreignKeyPropertyName, 
        List<Guid> parentIds, CancellationToken cancellationToken) where TChild: class, IDomainEntityMeta 
    {
        return Set<TChild>()
            .Where(child => parentIds.Contains(
                Microsoft.EntityFrameworkCore.EF.Property<Guid>(
                    child, foreignKeyPropertyName)) &&
                    !child.IsIgnored)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(
                        child => child.IsIgnored, true), cancellationToken);
    }
    private static bool IsExcludedFromIgnoreCascade(Type type)
    {
        return type == typeof(DriverAndDriverLicenseCategory);
    }

    } 