using FluentValidation;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class ScheduleValidator : AbstractValidator<CreateScheduleViewModel>
    
    {
        public ScheduleValidator()
        {
            RuleFor(s => s.Vehicle).NotNull();
            
        }
    }
}
