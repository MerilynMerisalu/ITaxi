using System.ComponentModel;
using App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteAdminViewModel
{
    public Guid Id { get; set; }
    [DisplayName("Personal Identifier")]
    public string PersonalIdentifier { get; set; } = default!;

    public City? City { get; set; }

    public string Address { get; set; } = default!;

}