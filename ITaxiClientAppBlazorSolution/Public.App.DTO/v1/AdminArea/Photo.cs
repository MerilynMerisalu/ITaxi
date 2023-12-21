using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.AdminArea
{
    public class Photo
    {
        public string Title { get; set; } = default!;
        public string? PhotoURL { get; set; }
        public Guid VehicleId { get; set; }
    }
}
