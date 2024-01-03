using Public.App.DTO.v1.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.Identity
{
    public class RegisterDriverJWTResponse: JwtResponse
    {
        public Driver? Driver { get; set; }
    }
}
