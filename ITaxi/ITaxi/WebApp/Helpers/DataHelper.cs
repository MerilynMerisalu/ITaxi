using System.Diagnostics;
using System.Security.Claims;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.Enum.Enum;
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

        using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

        if (context == null) throw new ApplicationException("Problem in services. No db context.");

        // TODO - Check database state
        // can't connect - wrong address
        // can't connect - wrong user/pass
        // can connect - but no database
        // can connect - there is database

        // userManager and roleManager



        if (userManager == null || roleManager == null) Console.Write("Cannot instantiate userManager or rolemanager!");

        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
            await context.Database.EnsureDeletedAsync();

        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase")) await context.Database.MigrateAsync();

        await SeedDatabase(context, userManager, roleManager,
            configuration.GetValue<bool>("DataInitialization:SeedIdentity"),
            configuration.GetValue<bool>("DataInitialization:SeedData"));
    }

    public static async Task SeedDatabase(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, bool seedIdentity, bool seedData)
    {
        if(seedIdentity)
        {
            var roles = new (string name, string displayName)[]
            {
                ("Admin", "Administrator"),
                ("Driver", "Driver"),
                ("Customer", "Customer")
            };

            foreach (var roleInfo in roles)
            {
                var role = roleManager!.FindByNameAsync(roleInfo.name).Result;
                if (role == null)
                {
                    var identityResult = roleManager.CreateAsync(new AppRole
                    {
                        Name = roleInfo.name,
                        DisplayName = roleInfo.displayName
                    });
                    if (!identityResult.Result.Succeeded)
                        foreach (var identityError in identityResult.Result.Errors)
                            Console.WriteLine("Cant create role! Error: " + identityError.Description);
                }
            }
            /*var role = new AppRole
            {
                Name = "Admin"
            };
            role.NormalizedName = role.Name.ToUpper();*/
            /*var result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
                    */

            /*role = new AppRole
            {
                Name = "Driver"
            };
            role.NormalizedName = role.Name.ToUpper();*/
            /*result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
                    */

            /*role = new AppRole
            {
                Name = "Customer"
            };
            role.NormalizedName = role.Name.ToUpper();*/
            /*result = roleManager!.CreateAsync(role).Result;
            if (!result.Succeeded)
                foreach (var identityError in result.Errors)
                    Console.WriteLine("Cant create role! Error: " + identityError.Description);
        }*/

            if (seedData)
            {
                // Initialize all vehicle Types
                //App.Resources.Areas.App.Domain.AdminArea.VehicleType.
                var regularVehicleType = new VehicleType
                {
                    Id = Guid.NewGuid(),
                    VehicleTypeName = App.Resources.Areas.App.Domain.AdminArea.VehicleType.Regular,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                regularVehicleType.VehicleTypeName.SetTranslation("Tava", "et-EE");
                await context.VehicleTypes.AddAsync(regularVehicleType);
                var wheelChairVehicleType = new VehicleType
                {
                    Id = Guid.NewGuid(),
                    VehicleTypeName = App.Resources.Areas.App.Domain.AdminArea.VehicleType.Wheelchair,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                wheelChairVehicleType.VehicleTypeName.SetTranslation("Inva", "et-EE");
                await context.VehicleTypes.AddAsync(wheelChairVehicleType);
                await context.SaveChangesAsync();

                var testVehicleType =
                    context.VehicleTypes.Include(x => x.VehicleTypeName.Translations).FirstOrDefault();
                Debug.WriteLine(testVehicleType);
                var disabilityType = new DisabilityType
                {
                    Id = Guid.NewGuid(),
                    DisabilityTypeName = "None",
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    CreatedBy = "System",
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
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Counties.AddAsync(county);
                await context.SaveChangesAsync();

                var city = new City
                {
                    Id = new Guid(),
                    CityName = "Tallinn",
                    CountyId = county.Id,
                    CreatedBy = "System",
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
                    PhoneNumber = "22356891",
                    
                };
                appUser.UserName = appUser.Email;

                var result = userManager!.CreateAsync(appUser, "Katrinkass123$").Result;
#warning ask if this is the right way to add a claim in my app context
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));


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
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    CreatedBy = "System"
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

#warning ask if this is the right way to add a claim in my app context
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));


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
                    CreatedBy = "System",
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
#warning ask if this is the right way to add a claim in my app context
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));


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
                    CreatedBy = "System",
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
#warning ask if this is the right way to add a claim in my app context
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));


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
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    CreatedBy = "System"
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
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.VehicleMarks.AddAsync(vehicleMark);
                await context.SaveChangesAsync();
                var vehicleModel = new VehicleModel
                {
                    Id = new Guid(),
                    VehicleModelName = "Avensis",
                    VehicleMarkId = vehicleMark.Id,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.VehicleModels.AddAsync(vehicleModel);
                await context.SaveChangesAsync();

                var vehicle = new Vehicle
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!
                        .Equals("38806237921")).Result.Id,
                    VehicleMarkId = context.VehicleMarks.SingleOrDefaultAsync(v =>
                        v.VehicleMarkName.Equals("Toyota")).Result!.Id,
                    VehicleTypeId = regularVehicleType.Id,
                    VehicleModelId = context.VehicleModels
                        .SingleOrDefaultAsync(v => v.VehicleModelName.Equals("Avensis")).Result!.Id,
                    ManufactureYear = 2020,
                    NumberOfSeats = 5,
                    VehiclePlateNumber = "555 XXZ",
                    VehicleAvailability = VehicleAvailability.Available,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Vehicles.AddAsync(vehicle);
                await context.SaveChangesAsync();


                vehicleMark = new VehicleMark
                {
                    Id = new Guid(),
                    VehicleMarkName = "Ford",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.VehicleMarks.AddAsync(vehicleMark);
                await context.SaveChangesAsync();

                vehicleModel = new VehicleModel
                {
                    Id = new Guid(),
                    VehicleModelName = "Focus",
                    VehicleMarkId = context.VehicleMarks
                        .SingleOrDefaultAsync
                            (v => v.VehicleMarkName.Equals("Ford")).Result!.Id,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.VehicleModels.AddAsync(vehicleModel);
                await context.SaveChangesAsync();

                vehicle = new Vehicle
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!
                        .Equals("36605138911")).Result.Id,
                    VehicleMarkId = context.VehicleMarks
                        .SingleOrDefaultAsync(v => v.VehicleMarkName.Equals("Ford")).Result!.Id,
                    VehicleTypeId = wheelChairVehicleType.Id,
                    VehicleModelId = context.VehicleModels
                        .SingleOrDefaultAsync(v => v.VehicleModelName.Equals("Focus")).Result!.Id,
                    ManufactureYear = 2020,
                    NumberOfSeats = 4,
                    VehiclePlateNumber = "123 AAC",
                    VehicleAvailability = VehicleAvailability.Available,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Vehicles.AddAsync(vehicle);
                await context.SaveChangesAsync();

                var schedule = new Schedule
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!.Equals("38806237921")).Result
                        .Id,
                    VehicleId = context.Vehicles.SingleOrDefaultAsync(v => v.VehiclePlateNumber
                        .Equals("555 XXZ")).Result!.Id,
                    StartDateAndTime = DateTime.Now.AddHours(9).ToUniversalTime(),
                    EndDateAndTime = DateTime.Now.AddHours(16).ToUniversalTime(),
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Schedules.AddAsync(schedule);
                await context.SaveChangesAsync();

                schedule = new Schedule
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!.Equals("36605138911")).Result
                        .Id,
                    VehicleId = context.Vehicles.SingleOrDefaultAsync(v => v.VehiclePlateNumber.Equals("123 AAC"))
                        .Result!.Id,
                    StartDateAndTime = DateTime.Now.AddHours(10).ToUniversalTime(),
                    EndDateAndTime = DateTime.Now.AddHours(18).ToUniversalTime(),
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Schedules.AddAsync(schedule);
                await context.SaveChangesAsync();


                var rideTime1 = new RideTime
                {
                    DriverId = context.Drivers.SingleOrDefaultAsync(d =>
                        d.PersonalIdentifier!.Equals("38806237921")).Result!.Id,
                    ScheduleId = context.Schedules
                        .SingleOrDefaultAsync(s => s.Driver!.PersonalIdentifier!.Equals("38806237921"))
                        .Result!.Id,
                    RideDateTime = context.Schedules.FirstOrDefaultAsync(s =>
                        s.Driver!.PersonalIdentifier!.Equals("38806237921")).Result!
                        .StartDateAndTime.AddMinutes(45), //.ToUniversalTime(), CS: Suspect that the value is already UTC, so we don'ˇt need to translate against
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.RideTimes.AddAsync(rideTime1);

                rideTime1 = new RideTime
                {
                    DriverId = context.Drivers.SingleOrDefaultAsync(d =>
                        d.PersonalIdentifier!.Equals("38806237921")).Result!.Id,
                    ScheduleId = context.Schedules
                        .SingleOrDefaultAsync(s => s.Driver!.PersonalIdentifier!.Equals("38806237921"))
                        .Result!.Id,
                    RideDateTime = context.Schedules.FirstOrDefaultAsync(s =>
                        s.Driver!.PersonalIdentifier!.Equals("38806237921")).Result!
                        .StartDateAndTime, //.ToUniversalTime(), CS: Suspect that the value is already UTC, so we don'ˇt need to translate against
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.RideTimes.AddAsync(rideTime1);
                await context.SaveChangesAsync();


                var rideTime2 = new RideTime
                {
                    DriverId = context.Drivers.SingleOrDefaultAsync(d =>
                        d.PersonalIdentifier!.Equals("36605138911")).Result!.Id,
                    ScheduleId = context.Schedules
                        .SingleOrDefaultAsync(s => s.Driver!.PersonalIdentifier!.Equals("36605138911"))
                        .Result!.Id,
                    RideDateTime = context.Schedules.FirstOrDefaultAsync(s =>
                        s.Driver!.PersonalIdentifier!.Equals("36605138911")).Result!
                        .StartDateAndTime.AddMinutes(90), //.ToUniversalTime(), CS: the value is already UTC, so we don't need to translate again
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.RideTimes.AddAsync(rideTime2);

                rideTime2 = new RideTime
                {
                    DriverId = context.Drivers.SingleOrDefaultAsync(d =>
                        d.PersonalIdentifier!.Equals("36605138911")).Result!.Id,
                    ScheduleId = context.Schedules
                        .SingleOrDefaultAsync(s => s.Driver!.PersonalIdentifier!.Equals("36605138911"))
                        .Result!.Id,
                    RideDateTime = context.Schedules.FirstOrDefaultAsync(s =>
                        s.Driver!.PersonalIdentifier!.Equals("36605138911")).Result!
                        .StartDateAndTime, //.ToUniversalTime(), CS: the value is already UTC, so we don't need to translate again
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.RideTimes.AddAsync(rideTime2);
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
#warning ask if this is the right way to add a claim in my app context
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));


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
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                
                appUser = new AppUser
                {
                    Id = new Guid(),
                    FirstName = "Kristjan",
                    LastName = "Suursalu",
                    DateOfBirth = DateTime.Parse("2000-04-14"),
                    Gender = Gender.Male,
                    Email = "kristjan.suursalu@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "88452236"
                };
                appUser.UserName = appUser.Email;

                result = userManager!.CreateAsync(appUser, "Kristjankoer123$").Result;
#warning ask if this is the right way to add a claim in my app context
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
                result = await userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));


                if (!result.Succeeded)
                    foreach (var identityError in result.Errors)
                        Console.WriteLine("Cant create user! Error: " + identityError.Description);
                result = userManager.AddToRoleAsync(appUser, "Customer").Result;
                if (!result.Succeeded)
                    foreach (var identityError in result.Errors)
                        Console.WriteLine("Cant add user to role! Error: " + identityError.Description);

                customer = new Customer
                {
                    Id = new Guid(),
                    AppUserId = context.Users.OrderBy(u => u.LastName).First(a =>
                        a.FirstName.Equals("Kristjan") && a.LastName.Equals("Suursalu")).Id,
                    DisabilityTypeId = disabilityType.Id,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();


                var booking = new Booking
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!.Equals("38806237921")).Result.Id,
                    VehicleId = context.Vehicles.FirstOrDefaultAsync(v => v.Driver!.PersonalIdentifier!.Equals("38806237921"))
                        .Result!.Id,
                    CustomerId = context.Customers.Include(c => c.AppUser)
                        .SingleOrDefaultAsync(c => c.AppUser!.FirstName.Equals("Maarika") 
                                                   && c.AppUser.LastName.Equals("Mätas"))
                                                    .Result!.Id,
                    CityId = city.Id,
                    ScheduleId = context.Schedules.Include(s => s.Driver)
                        .SingleOrDefaultAsync(s => s.Driver!.PersonalIdentifier!.Equals("38806237921"))
                        .Result!.Id,
                    VehicleTypeId = regularVehicleType.Id,
                    PickupAddress = "Suursõjamäe 15-2",
                    DestinationAddress = "Sõpruse pst 10",
                    PickUpDateAndTime = DateTime.Now.ToUniversalTime(),
                    NumberOfPassengers = 2,
                    HasAnAssistant = true,
                    StatusOfBooking = StatusOfBooking.Awaiting,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                };
                booking.UpdatedBy = booking.CreatedBy;
                booking.UpdatedAt = booking.CreatedAt;
                
                rideTime1.Booking = booking;
                rideTime1.IsTaken = true;
                booking.PickUpDateAndTime = rideTime1.RideDateTime;

                await context.Bookings.AddAsync(booking);
                await context.SaveChangesAsync();

                var drive = new Drive
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!.Equals("38806237921")).Result
                        .Id,
                    Booking = booking,
                    StatusOfDrive = StatusOfDrive.Awaiting,
                    
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Drives.AddAsync(drive);
                await context.SaveChangesAsync();

                booking = new Booking
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!.Equals("36605138911")).Result.Id,
                    VehicleId = context.Vehicles.FirstOrDefaultAsync(v => v.Driver!.PersonalIdentifier!.Equals("36605138911"))
                        .Result!.Id,
                    CustomerId = context.Customers.Include(c => c.AppUser)
                        .SingleOrDefaultAsync(c => c.AppUser!.FirstName.Equals("Kristjan") 
                                                   && c.AppUser.LastName.Equals("Suursalu"))
                                                    .Result!.Id,
                    CityId = city.Id,
                    ScheduleId = context.Schedules.Include(s => s.Driver)
                        .SingleOrDefaultAsync(s => s.Driver!.PersonalIdentifier!.Equals("36605138911"))
                        .Result!.Id,
                    VehicleTypeId = wheelChairVehicleType.Id,
                    PickupAddress = "Sõpruse 13-4",
                    DestinationAddress = "Kalamaja 10",
                    PickUpDateAndTime = DateTime.Now.ToUniversalTime(),
                    NumberOfPassengers = 2,
                    HasAnAssistant = true,
                    StatusOfBooking = StatusOfBooking.Awaiting,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                booking.UpdatedBy = booking.CreatedBy;
                booking.UpdatedAt = booking.CreatedAt;
                
                rideTime2.Booking = booking;
                rideTime2.IsTaken = true;
                booking.PickUpDateAndTime = rideTime2.RideDateTime;

                await context.Bookings.AddAsync(booking);
                await context.SaveChangesAsync();

                 drive = new Drive
                {
                    Id = new Guid(),
                    DriverId = context.Drivers.SingleAsync(d => d.PersonalIdentifier!.Equals("36605138911")).Result
                        .Id,
                    Booking = context.Bookings.Include(b => b.Driver)
                        .SingleOrDefaultAsync(b => b.Driver!.PersonalIdentifier!.Equals("36605138911")).Result!,
                    StatusOfDrive = StatusOfDrive.Awaiting,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Drives.AddAsync(drive);
                await context.SaveChangesAsync();

                var comment = new Comment
                {
                    Id = new Guid(),
                    CommentText = "Jäin teenusega rahule!",
                    Drive = await context.Drives.Include(d => d.Driver)
                        .SingleOrDefaultAsync(d => d.Driver!.PersonalIdentifier!.Equals("38806237921")),
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Comments.AddAsync(comment);
                await context.SaveChangesAsync();
                
                 comment = new Comment
                {
                    Id = new Guid(),
                    CommentText = "Takso hilines",
                    Drive = await context.Drives.Include(d => d.Driver)
                        .SingleOrDefaultAsync(d => d.Driver!.PersonalIdentifier!.Equals("36605138911")),
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };
                await context.Comments.AddAsync(comment);
                await context.SaveChangesAsync();
            }
        }
    }
}