using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditScheduleViewModel
{
    public Guid Id { get; set; }
    
    [DisplayName(nameof(Vehicle))]

    public Guid VehicleId { get; set; }


    [DataType(DataType.DateTime)]
    [DisplayName("Start Date and Time")]
    
    public DateTime StartDateAndTime { get; set; } 
    
    [DataType(DataType.DateTime)]
    [DisplayName("End Date and Time")]
    
    public DateTime EndDateAndTime { get; set; }

    public SelectList? Vehicles { get; set; }
    
}