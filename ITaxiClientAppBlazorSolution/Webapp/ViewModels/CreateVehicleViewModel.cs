using FluentValidation;
using ITaxi.Enum.Enum;
using ITaxi.Public.DTO.v1.AdminArea;

namespace Webapp.ViewModels
{
    public class CreateVehicleViewModel
    {
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

    public class CreateVehicleViewModelValidator : AbstractValidator<CreateVehicleViewModel>
    {
        public CreateVehicleViewModelValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Mark).NotEmpty();
            RuleFor(x => x.Model).NotEmpty();
            RuleFor(x => x.VehicleManufactureYear).NotEmpty();
            RuleFor(x => x.VehicleAvailability).NotEmpty();
            RuleFor(x => x.VehiclePlateNumber).NotEmpty().MaximumLength(10);
        }
    }
}
