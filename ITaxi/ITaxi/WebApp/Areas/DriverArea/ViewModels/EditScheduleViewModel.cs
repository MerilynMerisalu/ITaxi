using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

public class EditScheduleViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]

    public DateTime StartDateAndTime { get; set; } = default!;


    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]

    public DateTime EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }
}