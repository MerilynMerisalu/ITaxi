using System.Collections;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Claims;
using App.BLL;
using App.DAL.EF;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
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

        // TODO: could use Moq, but the logs are not useful in the test scenarios
        var controllerLogger = new NullLogger<CountiesController>();

        #endregion Global Setup
        
        // Instantiate the controller
        _controller = new CountiesController(appBll, v1Mapper, controllerLogger);
        
        #region HttpContext Setup (Permissions)
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Email, "xUnit@test.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Name, "xUnit Tester")
            }));

        var httpContext = new DefaultHttpContext()
        {
            User = user
        };
        httpContext.Features.Set(new TestApiVersionFeature());
        _controller.ControllerContext = new ControllerContext(new ActionContext
        {
            HttpContext = httpContext, 
            RouteData = new RouteData(),ActionDescriptor = new ControllerActionDescriptor()
        });
        #endregion HttpContext Setup (Permissions)
        
        // setup the data scenario (seed)
        Seed(_ctx); 
    }

    public class TestApiVersionFeature : IApiVersioningFeature
    {
        public string? RouteParameter { get; set; }
        public IReadOnlyList<string> RawRequestedApiVersions { get; set; } = new[] { "1.0" }.AsReadOnly();
        public string? RawRequestedApiVersion { get; set; } = "1.0";
        public ApiVersion? RequestedApiVersion { get; set; } = new ApiVersion(1, 0);
        public ActionSelectionResult SelectionResult { get; } = new ActionSelectionResult();
    }

    private void Seed(AppDbContext context)
    {
        // Here I use App.Domain types because this is the data context directly
        context.Counties.AddRange(new []{
            new App.Domain.County
            {
                Id = Guid.NewGuid(), CountyName = "County1", CreatedAt = DateTime.UtcNow, CreatedBy = "Me",
                UpdatedAt = DateTime.UtcNow, UpdatedBy = "Me", IsDeleted = false, Cities = new HashSet<App.Domain.City>()
            },
            new App.Domain.County
            {
                Id = Guid.NewGuid(), CountyName = "County2", CreatedAt = DateTime.UtcNow, CreatedBy = "Me",
                UpdatedAt = DateTime.UtcNow, UpdatedBy = "Me", IsDeleted = false, Cities = new HashSet<App.Domain.City>()
            }
        });
        
        context.SaveChanges();
    }
    
    [Fact]
    public async Task GetAllCountiesTests()
    {
        // Call the Controller to get the data
        var response = await _controller.GetCounties();
        var counties = Assert.IsType<OkObjectResult>(response.Result).Value as List<App.Public.DTO.v1.AdminArea.County>;
        
        // Now verify the results
        Assert.NotNull(counties);
        Assert.NotEmpty(counties);
    }

    [Fact]
    public async Task CreateACountyTests()
    {
        // Setup the data to send
        var county = new County
        {
            Id = Guid.NewGuid(),
            CountyName = "Test County",
        };

        // Call the Controller to POST the data
        var response = await _controller.PostCounty(county);
        Assert.NotNull(response);
        var typedResult = Assert.IsType<CreatedAtActionResult>(response.Result);
        Assert.Equal(201, typedResult.StatusCode);

        var newCounty = typedResult.Value as App.BLL.DTO.AdminArea.CountyDTO;
        Assert.NotNull(newCounty);
        // I expect the controller to assign an Id and ignore what we pass in.
        Assert.NotEqual(county.Id, newCounty.Id);
        // Use the Id from the response object to query the database so I can check the created by field
        var dbCounty = _ctx.Counties.First(x => x.Id == newCounty.Id);
        Assert.NotNull(dbCounty);
        Assert.NotEmpty(dbCounty.CreatedBy!);
        Assert.NotEqual(DateTime.MinValue, dbCounty.CreatedAt);
        
        
        // Additional test: check that we can post without an ID!
        var countyNoId = new County
        {
            CountyName = "Test County",
        };
        var noIdResponse = await _controller.PostCounty(countyNoId);
        Assert.NotNull(noIdResponse);
        var noIdCreatedResponse = Assert.IsType<CreatedAtActionResult>(noIdResponse.Result);
        Assert.Equal(201, noIdCreatedResponse.StatusCode);
        var countyWithAssignedId = noIdCreatedResponse.Value as App.BLL.DTO.AdminArea.CountyDTO;
        Assert.NotNull(countyWithAssignedId);
        Assert.NotEqual(Guid.Empty, countyWithAssignedId.Id);
    }
    
    [Fact]
    public async Task EditACountyTests()
    {
        // Get a record from the database directly
        // The other tests will ensure that the GET methods work, we don't need to test that
        var countyId = _ctx.Counties.Select(x => x.Id).First();
        // Make some changes!
        var countyChanges = new County
        {
            CountyName = "MyNewCounty"
        };
        
        // Call the Controller to POST the data
        // but record the timestamp first!
        var sentAt = DateTime.UtcNow;
        var response = await _controller.PutCounty(countyId, countyChanges);
        Assert.NotNull(response);
        
        // Get the object from the database to check
        var dbCounty = _ctx.Counties.First(x => x.Id == countyId);
        Assert.False(String.IsNullOrEmpty(dbCounty.UpdatedBy));
        Assert.NotEqual(dbCounty.CreatedBy, dbCounty.UpdatedBy);
        Assert.NotEqual(DateTime.MinValue, dbCounty.UpdatedAt);
        Assert.NotEqual(dbCounty.CreatedAt, dbCounty.UpdatedAt);
        Assert.True(dbCounty.CreatedAt < sentAt);
        Assert.True(dbCounty.UpdatedAt > sentAt);
    }

    [Fact]
    public async Task DeleteOneCountiesTests()
    {
        // Call the Controller to get the data
        var response = await _controller.GetCounties();
        var counties = Assert.IsType<OkObjectResult>(response.Result).Value as List<App.Public.DTO.v1.AdminArea.County>;
        
        Assert.NotNull(counties);
        Assert.NotEmpty(counties);
        
        var countyToDelete = counties.First();
        // Now verify the results

        var deleteResponse = await _controller.DeleteCounty(countyToDelete.Id);
        
        // Now check, if we get all counties, there should be only 1
        var checkResponse = await _controller.GetCounties();
        var countyResponse = Assert.IsType<OkObjectResult>(response.Result);
        var checkCounties = countyResponse.Value as List<App.Public.DTO.v1.AdminArea.County>;
        
        Assert.NotNull(countyToDelete);
        
        // The check rule here, is that the final count, should be the original count - 1.
        Assert.Equal(counties.Count, checkCounties!.Count);

        // Finally, check that the record actually exists, but has been soft deleted
        var deletedRecord = _ctx.Counties.FirstOrDefault(county => county.Id == countyToDelete.Id);
        Assert.NotNull(deletedRecord);
        Assert.True(deletedRecord.IsDeleted);
        Assert.NotNull(deletedRecord.DeletedAt);
        Assert.NotNull(deletedRecord.DeletedBy);
        Assert.NotEmpty(deletedRecord.DeletedBy);
    }

}
