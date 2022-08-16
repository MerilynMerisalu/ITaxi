using System.Diagnostics;
using App.DAL.EF;
using App.Domain;
using App.Domain.Enum;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Helpers;

public static class DataHelper
{
    public static async Task SetupAppData(IApplicationBuilder app, IWebHostEnvironment env,
        IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        await using var context = serviceScope
            .ServiceProvider.GetService<AppDbContext>();

        if (context == null) throw new ApplicationException("Problem in services. No db context.");

        // TODO - Check database state
        // can't connect - wrong address
        // can't connect - wrong user/pass
        // can connect - but no database
        // can connect - there is database

        // userManager and roleManager

        using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

        if (userManager == null || roleManager == null) Console.Write("Cannot instantiate userManager or rolemanager!");

        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
            await context.Database.EnsureDeletedAsync();

        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase")) await context.Database.MigrateAsync();

        if (configuration.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            var role = new AppRole
            {
                Name = "Admin"
            };
            role.NormalizedName = role.Name.ToUpper();
            var result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);

            role = new AppRole
            {
                Name = "Driver"
            };
            role.NormalizedName = role.Name.ToUpper();
            result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);

            role = new AppRole
            {
                Name = "Customer"
            };
            role.NormalizedName = role.Name.ToUpper();
            result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
        }

        if (configuration.GetValue<bool>("DataInitialization:SeedData"))
        {
            // Initialize all vehicle Types
            //App.Resources.Areas.App.Domain.AdminArea.VehicleType.
            var regularVehicleType = new VehicleType
            {
                Id = Guid.NewGuid(),
                VehicleTypeName = App.Resources.Areas.App.Domain.AdminArea.VehicleType.Regular,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            regularVehicleType.VehicleTypeName.SetTranslation("Tava", "et-EE");
            await context.VehicleTypes.AddAsync(regularVehicleType);
            var wheelChairVehicleType = new VehicleType
            {
                Id = Guid.NewGuid(),
                VehicleTypeName = App.Resources.Areas.App.Domain.AdminArea.VehicleType.Wheelchair,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            wheelChairVehicleType.VehicleTypeName.SetTranslation("Inva", "et-EE");
            await context.VehicleTypes.AddAsync(wheelChairVehicleType);
            await context.SaveChangesAsync();

            var testVehicleType = context.VehicleTypes.Include(x => x.VehicleTypeName.Translations).FirstOrDefault();
            Debug.WriteLine(testVehicleType);
            var disabilityType = new DisabilityType
            {
                Id = Guid.NewGuid(),
                DisabilityTypeName = "None",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            disabilityType.DisabilityTypeName.SetTranslation("Puudub", "et-EE");
            await context.DisabilityTypes.AddAsync(disabilityType);
            await context.SaveChangesAsync();

            var testDisabilityType =
                context.DisabilityTypes.Include(x => x.DisabilityTypeName.Translations).FirstOrDefault();
            Debug.WriteLine(testDisabilityType);

            var county = new County
            {
                Id = new Guid(),
                CountyName = "Harjumaa",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Counties.AddAsync(county);
            await context.SaveChangesAsync();

            var city = new City
            {
                Id = new Guid(),
                CityName = "Tallinn",
                CountyId = county.Id,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Cities.AddAsync(city);
            await context.SaveChangesAsync();

            var appUser = new AppUser
            {
                Id = new Guid(),
                FirstName = "Katrin",
                LastName = "Salu",
                DateOfBirth = DateTime.Parse("1992-08-20"),
                Gender = Gender.Female,
                Email = "kati@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "22356891"
            };
            appUser.UserName = appUser.Email;

            var result = userManager!.CreateAsync(appUser, "Katrinkass123$").Result;

            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create user! Error: " + identityError.Description);
            result = userManager.AddToRoleAsync(appUser, "Admin").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);

            result = userManager.AddToRoleAsync(appUser, "Admin").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);


            var admin = new Admin
            {
                Id = new Guid(),
                AppUserId = context.Users.OrderBy(u => u.LastName).First(a =>
                    a.FirstName.Equals("Katrin") && a.LastName.Equals("Salu")).Id,
                CityId = context.Cities.OrderBy(c => c.CityName).First().Id,
                PersonalIdentifier = "49208202221",
                Address = "Kalda 23",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();

            appUser = new AppUser
            {
                Id = new Guid(),
                FirstName = "Tiina",
                LastName = "Pilv",
                DateOfBirth = DateTime.Parse("1977-08-22"),
                Gender = Gender.Female,
                Email = "tiina.pilv@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "22356891"
            };
            appUser.UserName = appUser.Email;

            result = userManager!.CreateAsync(appUser, "Tiinakass123$").Result;

            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create user! Error: " + identityError.Description);
            result = userManager.AddToRoleAsync(appUser, "Admin").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);

            result = userManager.AddToRoleAsync(appUser, "Admin").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);


            admin = new Admin
            {
                Id = new Guid(),
                AppUserId = context.Users.OrderBy(u => u.LastName).First(a =>
                    a.FirstName.Equals("Tiina") && a.LastName.Equals("Pilv")).Id,
                CityId = context.Cities.OrderBy(c => c.CityName).First().Id,
                PersonalIdentifier = "47708222221",
                Address = "Suurmäe 13-9",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();


            appUser = new AppUser
            {
                Id = new Guid(),
                FirstName = "Toomas",
                LastName = "Paju",
                DateOfBirth = DateTime.Parse("1988-06-23"),
                Gender = Gender.Male,
                Email = "toomas.paju@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "55358834"
            };
            appUser.UserName = appUser.Email;

            result = userManager!.CreateAsync(appUser, "Toomaskoer123$").Result;

            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create user! Error: " + identityError.Description);
            result = userManager.AddToRoleAsync(appUser, "Driver").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);

            result = userManager.AddToRoleAsync(appUser, "Driver").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);
            var driver = new Driver
            {
                Id = new Guid(),
                AppUserId = context.Users.OrderBy(u => u.LastName).First(a =>
                    a.FirstName.Equals("Toomas") && a.LastName.Equals("Paju")).Id,
                CityId = context.Cities.OrderBy(c => c.CityName).First().Id,
                PersonalIdentifier = "38806237921",
                DriverLicenseNumber = "AAC 123",
                DriverLicenseExpiryDate = DateTime.Parse("2026-09-22"),
                Address = "Veerenni 13",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Drivers.AddAsync(driver);
            await context.SaveChangesAsync();

            var driverLicenseCategory = new DriverLicenseCategory
            {
                Id = new Guid(),
                DriverLicenseCategoryName = "B2",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };

            await context.DriverLicenseCategories.AddAsync(driverLicenseCategory);
            await context.SaveChangesAsync();

            var driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategory
            {
                DriverId = driver.Id,
                DriverLicenseCategoryId = driverLicenseCategory.Id
            };
            await context.DriverAndDriverLicenseCategories.AddAsync(driverAndDriverLicenseCategory);
            await context.SaveChangesAsync();


             appUser = new AppUser
            {
                Id = new Guid(),
                FirstName = "Peep",
                LastName = "Tolmusk",
                DateOfBirth = DateTime.Parse("1966-05-13"),
                Gender = Gender.Male,
                Email = "peep.tolmusk@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "22447799"
            };
            appUser.UserName = appUser.Email;

            result = userManager!.CreateAsync(appUser, "Peepkoer123$").Result;

            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create user! Error: " + identityError.Description);
            result = userManager.AddToRoleAsync(appUser, "Driver").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);

            result = userManager.AddToRoleAsync(appUser, "Driver").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description); 
            driver = new Driver
            {
                Id = new Guid(),
                AppUserId = context.Users.OrderBy(u => u.LastName).First(a =>
                    a.FirstName.Equals("Peep") && a.LastName.Equals("Tolmusk")).Id,
                CityId = context.Cities.OrderBy(c => c.CityName).First().Id,
                PersonalIdentifier = "36605138911",
                DriverLicenseNumber = "BCC 445",
                DriverLicenseExpiryDate = DateTime.Parse("2028-09-22"),
                Address = "Pelguranna 13 - 5",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Drivers.AddAsync(driver);
            await context.SaveChangesAsync();
            
            driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategory
            {
                DriverId = driver.Id,
                DriverLicenseCategoryId = driverLicenseCategory.Id
            };
            await context.DriverAndDriverLicenseCategories.AddAsync(driverAndDriverLicenseCategory);
            await context.SaveChangesAsync();


            
            var vehicleMark = new VehicleMark
            {
                Id = new Guid(),
                VehicleMarkName = "Toyota",
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.VehicleMarks.AddAsync(vehicleMark);
            await context.SaveChangesAsync();
            var vehicleModel = new VehicleModel
            {
                Id = new Guid(),
                VehicleModelName = "Avensis",
                VehicleMarkId = vehicleMark.Id,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.VehicleModels.AddAsync(vehicleModel);
            await context.SaveChangesAsync();

            var vehicle = new Vehicle
            {
                Id = new Guid(),
                DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!
                    .Equals("38806237921")).Result.Id,
                VehicleMarkId = vehicleMark.Id,
                VehicleTypeId = regularVehicleType.Id,
                VehicleModelId = vehicleModel.Id,
                ManufactureYear = 2020,
                NumberOfSeats = 4,
                VehiclePlateNumber = "139 AAC",
                VehicleAvailability = VehicleAvailability.Available,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Vehicles.AddAsync(vehicle);
            await context.SaveChangesAsync();

            var schedule = new Schedule
            {
                Id = new Guid(),
                DriverId = driver.Id,
                VehicleId = vehicle.Id,
                StartDateAndTime = DateTime.Now.AddHours(10),
                EndDateAndTime = DateTime.Now.AddHours(18),
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Schedules.AddAsync(schedule);
            await context.SaveChangesAsync();

            var rideTime = new RideTime
            {
                ScheduleId = schedule.Id,
                Driver = schedule.Driver,
                RideDateTime = schedule.StartDateAndTime.AddMinutes(45).ToUniversalTime(),
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.RideTimes.AddAsync(rideTime);
            await context.SaveChangesAsync();

            appUser = new AppUser
            {
                Id = new Guid(),
                FirstName = "Maarika",
                LastName = "Mätas",
                DateOfBirth = DateTime.Parse("2001-02-14"),
                Gender = Gender.Female,
                Email = "maarika.matas@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "66357754"
            };
            appUser.UserName = appUser.Email;

            result = userManager!.CreateAsync(appUser, "Maarikakass123$").Result;

            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create user! Error: " + identityError.Description);
            result = userManager.AddToRoleAsync(appUser, "Customer").Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant add user to role! Error: " + identityError.Description);

            var customer = new Customer
            {
                Id = new Guid(),
                AppUserId = context.Users.OrderBy(u => u.LastName).First(a =>
                    a.FirstName.Equals("Maarika") && a.LastName.Equals("Mätas")).Id,
                DisabilityTypeId = disabilityType.Id,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            var booking = new Booking
            {
                Id = new Guid(),
                DriverId = driver.Id,
                VehicleId = vehicle.Id,
                CustomerId = customer.Id,
                CityId = city.Id,
                ScheduleId = schedule.Id,
                VehicleTypeId = regularVehicleType.Id,
                PickupAddress = "Kalamaja 5-12",
                DestinationAddress = "Suursõjamäe 10",
                PickUpDateAndTime = DateTime.Now.ToUniversalTime(),
                NumberOfPassengers = 2,
                HasAnAssistant = true,
                StatusOfBooking = StatusOfBooking.Awaiting,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            var drive = new Drive
            {
                Id = new Guid(),
                DriverId = driver.Id,
                Booking = booking,
                StatusOfDrive = StatusOfDrive.Awaiting,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Drives.AddAsync(drive);
            await context.SaveChangesAsync();

            var comment = new Comment
            {
                Id = new Guid(),
                CommentText = "Jäin teenusega rahule!",
                Drive = drive,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }
    }
}