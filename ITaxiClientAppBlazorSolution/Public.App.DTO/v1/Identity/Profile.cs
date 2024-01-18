using ITaxi.Enum.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.Identity
{
    public class Profile
    {
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public Gender Gender { get; set; }
        public DateTime DateTime { get; set; }
        public string PhoneNumber { get; set; } = default!;
    }
}
