using FluentValidation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webapp.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(20)
                .Matches("[a-z]").WithMessage("Password must include an uppercase letter")
                .Matches("[A-Z]").WithMessage("Password must include a lowercase letter")
                .Matches("[0-9]").WithMessage("Password must include a numeric character")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must include a non-alpha-numeric character");
            return options;
        }

        public static IRuleBuilder<T, DateTime?> PastDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of Birth cannot be greater than today's date.");
            return options;
        }
        public static IRuleBuilder<T, DateTime?> FutureDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Driver license expiry date cannot be less than today's date.");
            return options;
        }
    }
}
