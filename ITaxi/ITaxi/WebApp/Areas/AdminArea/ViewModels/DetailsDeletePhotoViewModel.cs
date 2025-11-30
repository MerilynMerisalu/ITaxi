using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete photo view model
/// </summary>
public class DetailsDeletePhotoViewModel: AdminAreaBaseViewModel
{
    /// <summary>
    /// Photo id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Photo title
    /// </summary>
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Title))]
    public string Title { get; set; } = default!;

    
    /// <summary>
    /// Photo url
    /// </summary>

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
    public string PhotoURL { get; set; } = default!;
    /// <summary>
    /// Photo full path
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoFullPath))]
    public string PhotoFullPath { get; set; } = default!;
}