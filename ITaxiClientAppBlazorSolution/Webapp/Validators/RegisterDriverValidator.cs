using FluentValidation;
using Webapp.Extensions;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class RegisterDriverValidator: AbstractValidator<RegisterDriverViewModel>
    {
        public RegisterDriverValidator()
        {
            RuleFor(d => d.FirstName).NotEmpty();
            RuleFor(d => d.FirstName).MinimumLength(1);
            RuleFor(d => d.FirstName).MaximumLength(50);
            RuleFor(d => d.LastName).NotEmpty();
            RuleFor(d => d.LastName).MinimumLength(1);
            RuleFor(d => d.LastName).MaximumLength(50);
            RuleFor(d => d.Gender).NotNull();
            RuleFor(d => d.DateOfBirth).PastDate();
            RuleFor(d => d.PersonalIdentifier).MaximumLength(11);
            RuleFor(d => d.City).NotNull();
           
        }
    }
}
