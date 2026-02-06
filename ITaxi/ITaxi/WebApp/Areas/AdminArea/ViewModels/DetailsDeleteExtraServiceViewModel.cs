using App.Resources.Areas.App.Domain.AdminArea;
using Azure.Core;
using Google.Apis.PeopleService.v1.Data;
using RESTCountries.NET.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class DetailsDeleteExtraServiceViewModel : AdminAreaBaseViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the extra service
        /// </summary>
        [Display(ResourceType = typeof(ExtraService),
        Name = nameof(ExtraServiceName))]
        public string ExtraServiceName { get; set; } = default!;
        /// <summary>
        /// Description of the extra service
        /// </summary>
        [Display(ResourceType = typeof(ExtraService),
        Name = nameof(Description))]
        public string Description { get; set; } = default!;
        /// <summary>
        /// Price of the extra service
        /// </summary>
        [Display(ResourceType = typeof(ExtraService),
        Name = nameof(Price))]
        public string Price { get; set; } = default!;

        /// <summary>
        /// Type of the extra service
        /// </summary>
        public string Type { get; set; } = default!;
    }
}
