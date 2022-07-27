using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

public class CreateScheduleViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]
    //[DisplayFormat(DataFormatString = "{0:g}")]
    public string StartDateAndTime { get; set; } = default!;

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public string EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }
}