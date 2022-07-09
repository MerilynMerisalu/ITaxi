﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleViewModel
{
    public Guid? Id { get; set; }
    [Display(ResourceType = typeof(Vehicle), Name = "Driver")]
    public string DriverFullName { get; set; } = default!;

    [Display(ResourceType = typeof(Vehicle), Name = "VehicleType")]
    public string VehicleType { get; set; } = default!;
    
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehicleMark))]
    public string VehicleMark { get; set; } = default!;
    
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehicleModel))]
    public string VehicleModel { get; set; } = default!;
    
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehiclePlateNumber))]
    public string VehiclePlateNumber { get; set; } = default!;
    [Display(ResourceType = typeof(Vehicle), Name = nameof(ManufactureYear))]
    public int ManufactureYear { get; set; }

    [Display(ResourceType = typeof(Vehicle), Name = nameof(NumberOfSeats))]
    public int NumberOfSeats { get; set; }

    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehicleAvailability))] 
    public VehicleAvailability VehicleAvailability { get; set; }





}