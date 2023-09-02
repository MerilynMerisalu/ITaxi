using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Admin area base view model
/// </summary>
public class AdminAreaBaseViewModel
{
    /// <summary>
    /// Created at
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; } = default!;

    /// <summary>
    /// Created by
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(CreatedBy))]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Updated at
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = default!;

    /// <summary>
    /// Updated by
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(UpdatedBy))]
    public string? UpdatedBy { get; set; } 
}