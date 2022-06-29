using System.ComponentModel.DataAnnotations;
using Base.Resources;

namespace App.Domain.Enum;

public enum Gender
{
    [Display(ResourceType = typeof(Common), Name = "Custom")]
    Custom = 1,
    [Display(ResourceType = typeof(Common), Name = "Female")]
    Female,
    [Display(ResourceType = typeof(Common), Name = "Male")]
    Male
}

public enum VehicleAvailability
{
    Available = 1,
    Occupied
}

public enum StatusOfBooking
{
    Awaiting = 1,
    Accepted,
    [Display(Name = "In-Progress")] InProgress,
    Finished,
    Cancelled
}

public enum IsActive
{   [Display(ResourceType = typeof(Common), Name = "Active")]
    Active = 1,
    [Display(ResourceType = typeof(Common), Name = "InActive")] InActive,
}