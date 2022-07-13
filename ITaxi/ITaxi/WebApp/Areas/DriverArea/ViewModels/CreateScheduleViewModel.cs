using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

public class CreateScheduleViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftStartDateAndTime")]
    //[DisplayFormat(DataFormatString = "{0:g}")]
    public string StartDateAndTime { get; set; } = default!;

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftEndDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public string EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }

}