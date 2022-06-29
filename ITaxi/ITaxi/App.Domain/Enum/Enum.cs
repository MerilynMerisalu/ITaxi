using System.ComponentModel.DataAnnotations;

namespace App.Domain.Enum;

public enum Gender
{
    Custom = 1,
    Female,
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
{   Active = 1,
    [Display(Name = "In-Active")] InActive,
}