using FluentAssertions;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;

namespace WebApp.Testing;

public class HomeControllerTests
{
    private readonly HomeController _homeController;
    public HomeControllerTests()
    {
        
        using var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = logFactory.CreateLogger<HomeController>();
        _homeController = new HomeController(logger);
    }
    [Fact]
    public void Test_TestAction()
    {
        // Arrange

        var a = 1;
        var b = 2;

        // Act

        var result = _homeController.TestAction(a, b);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.Equal(1, result.Length);
        Assert.Equal("3", result);
        
    }
    
    [Fact]
    public void TestFluent_TestAction()
    {
        // Arrange

        var a = 1;
        var b = 2;

        // Act

        var result = _homeController.TestAction(a, b);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.Equal(1, result.Length);
        Assert.Equal("4", result);
        result.Should().NotBeNull();
        string.IsNullOrWhiteSpace(result).Should().BeFalse();
        result.Length.Should().Be(1);
        result.Should().Be("3");
    }
}