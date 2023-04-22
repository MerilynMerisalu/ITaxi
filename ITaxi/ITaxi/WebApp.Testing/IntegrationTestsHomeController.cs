namespace WebApp.Testing;

public class IntegrationTestsHomeController
{
    private readonly HttpClient _client;

    public IntegrationTestsHomeController(HttpClient client)
    {
        _client = client;
    }

    [Fact]

    public async Task Get_Index()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/");
        
        // Assert

        response.EnsureSuccessStatusCode();
    }
}