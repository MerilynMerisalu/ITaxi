
using Base.Resources;
using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class CreateEditExtraServiceViewModel
    {
        /// <summary>
        /// Extra service entity id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Extra service name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [StringLength(maximumLength:128, MinimumLength = 1,ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        public string ExtraServiceName { get; set; } = default!;
        /// <summary>
        /// extra service description 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        public string Description { get; set; } = default!;
        /// <summary>
        /// Extra service price
        /// </summary>
        [Range(0, 10, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        public decimal Price { get; set; }
        /// <summary>
        /// Exrta service type
        /// </summary>
        public ExtraServiceType ExtraServiceType { get; set; }
    }
}
