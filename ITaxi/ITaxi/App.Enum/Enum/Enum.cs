using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

using Base.Resources;

namespace App.Enum.Enum;

public enum Gender
{
    [Display(ResourceType = typeof(Base.Resources.Common), Name = "Custom")]
    Custom = 1,

    [Display(ResourceType = typeof(Base.Resources.Common), Name = "Female")]
    Female = 2,

    [Display(ResourceType = typeof(Base.Resources.Common), Name = "Male")]
    Male= 3
}

public enum VehicleAvailability
{
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "Available")]
    Available = 1,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "InAvailable")]
    InAvailable
}

public enum StatusOfBooking
{
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Booking), Name = "AwaitingForConfirmation")]
    Awaiting = 1,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Booking), Name = "Accepted")]
    Accepted,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Booking), Name = "Declined")]
    Declined
}

public enum IsActive
{
    [Display(ResourceType = typeof(Base.Resources.Common), Name = "Active")]
    Active = 1,

    [Display(ResourceType = typeof(Base.Resources.Common), Name = "InActive")]
    InActive
}

public enum StatusOfDrive
{
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Drive), Name = "AwaitingForConfirmation")]
    Awaiting = 1,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Drive), Name = "Accepted")]
    Accepted,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Drive), Name = "Declined")]
    Declined,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Drive), Name = "InProgress")]
    Started,

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Drive), Name = "Finished")]
    Finished
}

public enum ExtraServiceType
{
    [Display(ResourceType = typeof(ExtraService), Name = "Driver")]
    Driver = 1,
    [Display(ResourceType = typeof(ExtraService), Name = "Vehicle")]
    Vehicle
}

public enum PhotoType
{
    [Display(ResourceType = typeof(Photo), Name = "Vehicle")]
    Vehicle = 1,
    [Display(ResourceType = typeof(Photo), Name = "ProfilePhoto")]
    Profile,
}

public enum DataOrigin
{
    [Display(ResourceType = typeof(Base.Resources.Base.Domain.Common), Name = "ManuallyAdded")]
    Manual = 1,
    [Display(ResourceType = typeof(Base.Resources.Base.Domain.Common), Name = "Imported")]
    Api
}