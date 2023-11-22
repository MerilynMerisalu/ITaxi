using ITaxi.Resources.Areas.App.Domain;
using System.ComponentModel.DataAnnotations;


namespace ITaxi.Enum.Enum;

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
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "Available")]
    Available = 1,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "InAvailable")]
    InAvailable
}

public enum StatusOfBooking
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "AwaitingForConfirmation")]
    Awaiting = 1,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "Accepted")]
    Accepted,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "Declined")]
    Declined
}

public enum IsActive
{
    [Display(ResourceType = typeof(Common), Name = "Active")]
    Active = 1,

    [Display(ResourceType = typeof(Common), Name = "InActive")]
    InActive
}

public enum StatusOfDrive
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "AwaitingForConfirmation")]
    Awaiting = 1,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "Accepted")]
    Accepted,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "Declined")]
    Declined,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "InProgress")]
    Started,

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "Finished")]
    Finished
}