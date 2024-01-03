using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.AdminArea
{
    public class DriverAndDriveLicenseCategory: Entity
    {
        public Guid DriverId { get; set; }
        public Guid DriverLicenseCategoryId { get; set; }
    }
}
