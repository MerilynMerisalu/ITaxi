using Base.Service.Contracts;
using System.Runtime.Serialization;

namespace BlazorWebApp.Services
{
    /// <summary>
    /// Typed response from OAuth 2.0 Services
    /// </summary>
    public class AuthResponse : IAuthResponse
    {
        /// <summary>
        /// Create a new OAuth response
        /// </summary>
        public AuthResponse()
        {
            this.Created = DateTimeOffset.Now;
        }
        /// <summary>
        /// The Token returned from the Authentication Provider
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Time that this token was created
        /// </summary>
        public DateTimeOffset Created { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstAndLastName { get; set; }
        public string LastAndFirstName { get; set; }
        public string[] RoleNames { get; set; }
    }

}
