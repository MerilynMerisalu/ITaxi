using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Enum;

namespace WebApp.Helpers;

public static class DataHelper
{
    public static async Task SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        using var serviceScope = app.
            ApplicationServices.
            GetRequiredService<IServiceScopeFactory>().
            CreateScope();

        using var context = serviceScope
            .ServiceProvider.GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services. No db context.");
        }
        
        // TODO - Check database state
        // can't connect - wrong address
        // can't connect - wrong user/pass
        // can connect - but no database
        // can connect - there is database
        
        // userManager and roleManager

        using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

        if (userManager == null || roleManager == null)
        {
            Console.Write("Cannot instantiate userManager or rolemanager!");
        }
        
        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            await context.Database.EnsureDeletedAsync();
        }
        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            await context.Database.MigrateAsync();
        }

        if (configuration.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            var role = new AppRole()
            {

                Name = "Admin",

            };
            role.NormalizedName = role.Name.ToUpper();
            var result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
                }

            }
            
            role = new AppRole()
            {

                Name = "Driver",

            };
            role.NormalizedName = role.Name.ToUpper();
            result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
                }

            }
            
            role = new AppRole()
            {

                Name = "Customer",

            };
            role.NormalizedName = role.Name.ToUpper();
            result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
                }

            }
        }

        if (configuration.GetValue<bool>("DataInitialization:SeedData"))
        {
            var county = new County()
            {
                Id = new Guid(),
                CountyName = "Harjumaa",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
             await context.Counties.AddAsync(county);
             await context.SaveChangesAsync();

            var city = new City()
            {
                Id = new Guid(),
                CityName = "Tallinn",
                CountyId =  context.Counties
                    .SingleOrDefaultAsync(c => c.CountyName.Equals("Harjumaa")).Result!.Id, 
                CreatedAt = DateTime.Now.ToUniversalTime()
                
            };
            await context.Cities.AddAsync(city);
            await context.SaveChangesAsync();

            var appUser = new AppUser()
            {
                Id = new Guid(),
                FirstName = "Katrin",
                LastName = "Salu",
                DateOfBirth = DateTime.Parse("20.08.1992"),
                Gender = Gender.Female,
                Email = "kati@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "22356891"

            };
            appUser.UserName = appUser.Email;

            var result = userManager!.CreateAsync(appUser, "Katrinkass123$").Result;
            
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    Console.WriteLine("Cant create user! Error: " + identityError.Description);
                }
            }
            result = userManager.AddToRoleAsync(appUser, "Admin").Result;
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);
                }
            }
            
            result = userManager.AddToRoleAsync(appUser, "Admin").Result;
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);
                }
            }

            
            var admin = new Admin()
            {
                Id = new Guid(),
                AppUserId = context.Users.OrderBy(u => u.LastName).First(a => 
                    a.FirstName.Equals("Katrin") && a.LastName.Equals("Salu")).Id,
                CityId = context.Cities.OrderBy(c => c.CityName).First().Id,
                PersonalIdentifier = "49208202221",
                Address = "Kalda 23", 
                CreatedBy = "System", 
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();




            var driverLicenseCategory = new DriverLicenseCategory()
            {
                Id = new Guid(),
                DriverLicenseCategoryName = "B2",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };

            await context.DriverLicenseCategories.AddAsync(driverLicenseCategory);
            await context.SaveChangesAsync();

            var disabilityType = new DisabilityType()
            {
                Id = new Guid(),
                DisabilityTypeName = "None",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.DisabilityTypes.AddAsync(disabilityType);
            await context.SaveChangesAsync();

            var vehicleType = new VehicleType()
            {
                Id = new Guid(),
                VehicleTypeName = "Regular",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.VehicleTypes.AddAsync(vehicleType);
            await context.SaveChangesAsync();

            var vehicleMark = new VehicleMark()
            {
                Id = new Guid(),
                VehicleMarkName = "Toyota",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.VehicleMarks.AddAsync(vehicleMark);
            await context.SaveChangesAsync();

        }
    }
    
}
