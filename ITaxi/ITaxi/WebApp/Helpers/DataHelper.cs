using App.DAL.EF;
using App.Domain;
using Microsoft.EntityFrameworkCore;

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
            // TODO
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
        }
    }
    
}
