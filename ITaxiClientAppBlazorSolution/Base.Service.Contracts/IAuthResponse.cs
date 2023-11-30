
namespace Base.Service.Contracts
{
    public interface IAuthResponse
    {
        DateTimeOffset Created { get; }
        string FirstAndLastName { get; set; }
        string FirstName { get; set; }
        string LastAndFirstName { get; set; }
        string LastName { get; set; }
        string RefreshToken { get; set; }
        string[] RoleNames { get; set; }
        string Token { get; set; }
    }
}