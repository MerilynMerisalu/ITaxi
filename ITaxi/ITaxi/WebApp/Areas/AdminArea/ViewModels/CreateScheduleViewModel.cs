using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateScheduleViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public string StartDateAndTime { get; set; } = default!;

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public string EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }
    
}