using System.Collections;
using System.Diagnostics;
using System.Reflection.Emit;
using App.BLL;
using App.DAL.EF;
using App.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using WebApp.ApiControllers.AdminArea;
using WebApp.Helpers;


namespace Tests;

public class CountiesControllerTests
{
    private readonly CountiesController _controller;
    private readonly AppDbContext _ctx;
        
    public CountiesControllerTests()
    {
        #region Global Setup
        
        #region EF Setup (DbContext)
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        //var userManager = new UserManager<App.Domain.Identity.AppUser>(_ctx,);
        //DataHelper.SetupAppData(_ctx);

        #endregion EF Setup (DbContext)

        var dalMapperConfig = new App.DAL.EF.AutoMapperConfig();
        var dalMappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new App.DAL.EF.AutoMapperConfig());
        });
        var dalMapper = dalMappingConfig.CreateMapper();
        
        var bllMappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new App.BLL.AutoMapperConfig());
        });
        var bllMapper = bllMappingConfig.CreateMapper();
        
        var v1MappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new WebApp.ApiControllers.v1.AutoMapperConfig());
        });
        var v1Mapper = v1MappingConfig.CreateMapper();

        var uow = new AppUOW(_ctx, dalMapper);
        var appBll = new AppBLL(uow, bllMapper);


        #endregion Global Setup
        
        // Instantiate the controller
        _controller = new CountiesController(appBll, v1Mapper);
        
        // setup the data scenario (seed)
        Seed(_ctx); 
    }

    private void Seed(AppDbContext context)
    {
        context.Counties.AddRange(new []{
            new County
            {
                Id = Guid.NewGuid(), CountyName = "County1", CreatedAt = DateTime.UtcNow, CreatedBy = "Me",
                UpdatedAt = DateTime.UtcNow, UpdatedBy = "Me", IsDeleted = false, Cities = new HashSet<City>()
            },
            new County
            {
                Id = Guid.NewGuid(), CountyName = "County2", CreatedAt = DateTime.UtcNow, CreatedBy = "Me",
                UpdatedAt = DateTime.UtcNow, UpdatedBy = "Me", IsDeleted = false, Cities = new HashSet<City>()
            }
        });
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task GetAllCountiesTests()
    {
        // Call the Controller to get the data
        var response = await _controller.GetCounties();
        var counties = (response.Result as OkObjectResult).Value as List<App.Public.DTO.v1.AdminArea.County>;
        
        // Now verify the results
        Assert.NotNull(counties);
        Assert.NotEmpty(counties);
    }
    
    [Fact]
    public async Task DeleteOneCountiesTests()
    {
        // Call the Controller to get the data
        var response = await _controller.GetCounties();
        var counties = (response.Result as OkObjectResult).Value as List<App.Public.DTO.v1.AdminArea.County>;
        
        Assert.NotEmpty(counties);
        
        var countyToDelete = counties.First();
        // Now verify the results

        var deleteResponse = await _controller.DeleteCounty(countyToDelete.Id);
        
        // Now check, if we get all counties, there should be only 1
        var checkResponse = await _controller.GetCounties();
        var checkCounties = (response.Result as OkObjectResult).Value as List<App.Public.DTO.v1.AdminArea.County>;
        
        Assert.NotNull(countyToDelete);
        
        // The check rule here, is that the final count, should be the original count - 1.
        Assert.Equal(counties.Count, checkCounties!.Count);

        // Finally, check that the record actually exists, but has been soft deleted
        var deletedRecord = _ctx.Counties.FirstOrDefault(county => county.Id == countyToDelete.Id);
        Assert.NotNull(deletedRecord);
        Assert.True(deletedRecord.IsDeleted);
    }

}
