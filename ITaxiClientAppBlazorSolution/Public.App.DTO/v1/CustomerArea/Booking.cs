using Base.Resources;
using ITaxi.Enum.Enum;
using Public.App.DTO.v1.AdminArea;
using Public.App.DTO.v1.DriverArea;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.CustomerArea
{
    public class Booking : Entity
    {
        public Driver? Driver { get; set; }
        public Guid CustomerId { get; set; }

        public Guid VehicleTypeId { get; set; }
        public VehicleType? VehicleType { get; set; }
        public Guid VehicleId { get; set; }
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(Vehicle))]
        public Vehicle? Vehicle { get; set; }
        public Guid CityId { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(City))]
        public City? City { get; set; }

        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = "PickUpDateAndTime")]
        public DateTime PickUpDateAndTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(PickupAddress))]
        public string PickupAddress { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName =
            "ErrorMessageMaxLength")]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(DestinationAddress))]
        public string DestinationAddress { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = "NumberOfPassengers")]
        public int NumberOfPassengers { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(HasAnAssistant))]
        public bool HasAnAssistant { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [DataType(DataType.MultilineText)]
        public string? AdditionalInfo { get; set; }
        public StatusOfBooking StatusOfBooking { get; set; }
        public string DriveTime => $"{PickUpDateAndTime:g}";

        public bool IsDeclined { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Booking), Name = "BookingDeclineDateAndTime")]
        public DateTime DeclineDateAndTime { get; set; }

        public string DeclineDateAndTimeCustomerView => $"{DeclineDateAndTime:g}";


    }
}
