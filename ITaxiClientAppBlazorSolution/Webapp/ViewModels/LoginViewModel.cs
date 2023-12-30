using BlazorWebApp.Services;
using System.ComponentModel.DataAnnotations;
using Webapp.Helpers;

namespace Webapp.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        [EmailAddress]
        public string Email { get; set; } = default!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        public string Password { get; set; } = default!;
        //[AuthorizeHelper]
        public bool? IsAuthenicated { get; set; } = false;
       //public string Token { get; set; }
    }
}
