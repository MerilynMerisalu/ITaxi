
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditScheduleViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    #warning Incorrect datetime formatting
    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = false)]
    public DateTime StartDateAndTime { get; set; } = default!;

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = false)]
    #warning Incorrect datetime formatting
    public DateTime EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }

}