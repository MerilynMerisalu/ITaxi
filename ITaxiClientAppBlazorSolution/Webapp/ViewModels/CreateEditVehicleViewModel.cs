using FluentValidation;
using ITaxi.Enum.Enum;
using Public.App.DTO.v1.AdminArea;
using Webapp.ViewModels;

namespace Webapp.ViewModels
{
    public class CreateEditVehicleViewModel
    {
        public Guid VehicleTypeId { get; set; }
        private VehicleMark? mark;

        public VehicleType? Type { get; set; }
        public VehicleMark? Mark
        {
            get => mark;
            set
            {
                mark = value;
                Model = null;
            }
        }

        public VehicleModel? Model { get; set; }

        public string VehiclePlateNumber { get; set; }

        public int? VehicleManufactureYear { get; set; }
        public int NumberOfSeats { get; set; } = 0;
        public VehicleAvailability? VehicleAvailability { get; set; }
    }

    
}

