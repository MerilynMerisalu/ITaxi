using App.DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebApp.ApiControllers.AdminArea;

namespace Tests.Unit;

public class ApiCountiesUnitTests
{
    [Fact(DisplayName = "GET - api/counties")]
    public void testGetAllCounties()
    {
        // arrange
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var _ctx = new AppDbContext(optionsBuilder.Options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();
        
        // SUT
        var controller = new CountiesController(_ctx,);
    }
}