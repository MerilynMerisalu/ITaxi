using FluentValidation;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class LoginValidator: AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            When(x => x.IsAuthenicated == false, () =>
            {
                RuleFor(x => x.Email).Must(x => false).WithMessage("invalid username or password");
                RuleFor(x => x.Password).Must(x => false).WithMessage("invalid username or password");
            }).Otherwise(() =>
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            });
        }
    }
}
