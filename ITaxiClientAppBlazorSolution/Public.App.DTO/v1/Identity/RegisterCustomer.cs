using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.Identity
{
    public class RegisterCustomer: Register
    {
        
        /// <summary>
        /// Disability type id for the customer registration
        /// </summary>
        
        public Guid DisabilityTypeId { get; set; }
    }
}
