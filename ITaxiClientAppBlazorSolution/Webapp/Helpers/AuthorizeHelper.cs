using BlazorWebApp.Services;
using System.ComponentModel.DataAnnotations;

namespace Webapp.Helpers
{
    public class AuthorizeHelper : ValidationAttribute
    {
        public AuthResponse? AuthResponse { get; set; }
        protected readonly string ErrorMessage = "Email and / or password wrong!";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (AuthResponse?.Token != null)
            {
                return null;
            }
            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
}
