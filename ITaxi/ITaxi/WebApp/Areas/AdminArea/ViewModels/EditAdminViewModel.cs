using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditAdminViewModel
{
    public Guid Id { get; set; }
    
    [StringLength(50)]
    [DisplayName("Personal Identifier")]
    public string? PersonalIdentifier { get; set; }

    [DisplayName("City")] public Guid CityId { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; } = default!;

    public SelectList? Cities { get; set; }

}