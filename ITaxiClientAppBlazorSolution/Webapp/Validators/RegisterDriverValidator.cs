using Base.Resources;
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
            RuleFor(d => d.DateOfBirth).PastDate("Date of Birth cannot be greater than today's date.");
            RuleFor(d => d.PersonalIdentifier).MaximumLength(11);
            RuleFor(d => d.City).NotNull();
            RuleFor(d => d.Address).NotNull();
            RuleFor(d => d.DriverLicenseNumber).NotEmpty();
            RuleFor(d => d.DriverLicenseExpiryDate).FutureDate("Driver license expiry date cannot be less than today's date.");
            RuleFor(d => d.DriverLicenseCategoriesForValidation)
                .Must((model, property) => model.SelectedDriverLicenseCategories.Any())
                .WithMessage("Select at least one license");
            //RuleFor(d => d.SelectedDriverLicenseCategories).NotNull();
            RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);
            RuleFor(c => c.EmailAddress).NotEmpty();
            RuleFor(c => c.EmailAddress).EmailAddress();
            RuleFor(c => c.Password).Password();
            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password).WithMessage(Common.ErrorMessageComparePasswords);
        }
    }
}
