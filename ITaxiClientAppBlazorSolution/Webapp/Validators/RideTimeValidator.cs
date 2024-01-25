using FluentValidation;
using Public.App.DTO.v1.DriverArea;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class RideTimeValidator: AbstractValidator<CreateRideTimeViewModel>
    {
        public RideTimeValidator()
        {
            RuleFor(rt => rt.Schedule).NotNull();
            RuleFor(rt => rt.RideTimeForValidation)
                .Must((model, property) => model.SelectedRideTimes.Any())
                .WithMessage("Select at least one ride time");
        }
    }
}
