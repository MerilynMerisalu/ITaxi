using Public.App.DTO.v1.AdminArea;

namespace Webapp.ViewModels
{
    public class RegisterDriverViewModel: RegisterViewModel
    {
        public string? PersonalIdentifier { get; set; }
        public City? City { get; set; }
    }
}
