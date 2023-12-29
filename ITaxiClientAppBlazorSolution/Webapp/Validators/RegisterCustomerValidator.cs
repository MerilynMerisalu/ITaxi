using Base.Resources;
using FluentValidation;
using System.Text.RegularExpressions;
using Webapp.Extensions;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class RegisterCustomerValidator : AbstractValidator<RegisterCustomerViewModel>
    {
        public RegisterCustomerValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.FirstName).MinimumLength(1);
            RuleFor(c => c.FirstName).MaximumLength(50);
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.LastName).MinimumLength(1);
            RuleFor(c => c.LastName).MaximumLength(50);
            RuleFor(c => c.Gender).NotNull();
            RuleFor(c => c.DateOfBirth).NotNull();
            RuleFor(c => c.DateOfBirth).LessThanOrEqualTo(DateTime.Today.Date);
            RuleFor(c => c.DisabilityType).NotEmpty();
            RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20)
            .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"));
            RuleFor(c => c.EmailAddress).NotEmpty();
            RuleFor(c => c.EmailAddress).EmailAddress();
            RuleFor(c => c.Password).Password();
            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password).WithMessage(Common.ErrorMessageComparePasswords);
        }
    }

}

 

