﻿using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DriveStateViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(Schedule), Name = "ScheduleName")]
    public string Schedule { get; set; } = default!;

    [Display(ResourceType = typeof(Schedule), Name = nameof(ShiftDurationTime))]
    public string ShiftDurationTime { get; set; } = default!;

    [Display(ResourceType = typeof(Driver), Name = "JobTitle")]
    public string DriverLastAndFirstName { get; set; } = default!;

    [Display(ResourceType = typeof(Customer), Name = "CustomerName")]
    public string CustomerLastAndFirstName { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = "PickUpDateAndTime")]
    public string PickupDateAndTime { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(City))]
    public string City { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = "Vehicle")]
    public string VehicleIdentifier { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(StatusOfBooking))]
    public StatusOfBooking StatusOfBooking { get; set; }
    
    [Display(ResourceType = typeof(Drive), Name = "DriveStatus")]
    public StatusOfDrive StatusOfDrive { get; set; }


    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
    
    [Display(ResourceType = typeof(Drive), Name = "AcceptedDateAndTime")]

    public string? DriveAcceptedDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "DeclineDateAndTime")]
    
    public string? DriveDeclineDateAndTime { get; set; }
    
    [Display(ResourceType = typeof(Drive), Name = "InProgressDateAndTime")]
    
    public string? DriveInProgressDateAndTime { get; set; }
    [Display(ResourceType = typeof(Drive), Name = "FinishedDateAndTime")]
    public string? DriveFinishedDateAndTime { get; set; }
    
    public bool IsDriveAccepted { get; set; }
    
    public bool IsDriveDeclined { get; set; }
    
    public bool IsDriveStarted { get; set; }
    
    public bool IsDriveFinished { get; set; }

}