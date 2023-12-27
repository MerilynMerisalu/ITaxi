using FluentValidation;
using Webapp.ViewModels;

public class VehicleValidator : AbstractValidator<CreateEditVehicleViewModel>
{
    public VehicleValidator()
    {
        
        RuleFor(v => v.Type).NotNull();
        RuleFor(v => v.Mark).NotNull();
        RuleFor(v => v.Model).NotNull();
        RuleFor(v => v.VehiclePlateNumber).NotEmpty();
        RuleFor(v => v.VehicleManufactureYear).NotEmpty();
        RuleFor(v => v.NumberOfSeats).GreaterThan(0);
        RuleFor(v => v.VehicleAvailability).NotNull();


    }
}




