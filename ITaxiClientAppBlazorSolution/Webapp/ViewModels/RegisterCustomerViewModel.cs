using ITaxi.Enum.Enum;
using Public.App.DTO.v1.AdminArea;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Webapp.ViewModels
{
    public class RegisterCustomerViewModel: RegisterViewModel
    {
        
        public DisabilityType? DisabilityType { get; set; }
        
    }
}
