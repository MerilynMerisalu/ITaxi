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
                RuleFor(x => x.Email).Must(x => false).WithMessage("Email and / or password wrong!");
                
            }).Otherwise(() =>
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            });
        }
    }
}
