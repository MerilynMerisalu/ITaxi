using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditCustomerViewModel
{
    public Guid Id { get; set; }
    [DisplayName("Disability Type")]
    public Guid DisabilityTypeId { get; set; }
   
    public SelectList? DisabilityTypes { get; set; } 
}