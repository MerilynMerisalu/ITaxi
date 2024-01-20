using FluentValidation;
using Webapp.Extensions;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class ScheduleValidator : AbstractValidator<CreateScheduleViewModel>
    
    {
        public ScheduleValidator()
        {
            RuleFor(s => s.Vehicle).NotNull();
            RuleFor(s => s.StartDateAndTime).FutureDateAndTime("Schedule start date and time cannot be less than today's date and current time.");
            RuleFor(s => s.EndDateAndTime).FutureDateAndTime("Schedule end date and time cannot be less than today's date and current time.")
                .GreaterThan(s  => DateTime.Parse(s.StartDateAndTime.Value.TimeOfDay.ToString()))
                .WithMessage("Schedule end time cannot be less or equal to schedule start time.");
            
        }

        
    }
}
