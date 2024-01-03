using Public.App.DTO.v1.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.Identity
{
    public class RegisterCustomerJWTResponse: JwtResponse
    {
        ///// <summary>
        ///// Token
        ///// </summary>
        //public string Token { get; set; } = default!;

        ///// <summary>
        ///// Refresh token
        ///// </summary>
        //public string RefreshToken { get; set; } = default!;

        ///// <summary>
        ///// Customer's first name
        ///// </summary>
        //public string FirstName { get; set; } = default!;

        ///// <summary>
        ///// Customer's last name
        ///// </summary>
        //public string LastName { get; set; } = default!;

        ///// <summary>
        ///// Customer's first anf last name
        ///// </summary>
        //public string FirstAndLastName => $"{FirstName} {LastName}";

        ///// <summary>
        ///// Customer's last and first name
        ///// </summary>
        //public string LastAndFirstName => $"{LastName} {FirstName}";

        ///// <summary>
        ///// Role names
        ///// </summary>
        //public string[] RoleNames { get; set; } = default!;

        /// <summary>
        /// Customer object
        /// </summary>
        public Customer? Customer { get; set; }
    }
}
