using FluentValidation;
using Webapp.ViewModels;

namespace Webapp.Validators
{
    public class LoginValidator: AbstractValidator<LoginViewModel>
    {
        public LoginValidator() { 
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.IsAuthenicated).Equal(false);
        }
    }
}
