using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.CustomerArea;

namespace WebApp.Areas.CustomerArea.ViewModels;

/// <summary>
/// Details delete comment view model
/// </summary>
public class DetailsDeleteCommentViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver name
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = "Driver")]
    public string DriverName { get; set; } = default!;

    /// <summary>
    /// Drive
    /// </summary>
    [Display(ResourceType = typeof(Comment),
        Name = nameof(Drive))]
    public string Drive { get; set; } = default!;
    
    /// <summary>
    /// Rating for the drive
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "Rating")]
    [Range(minimum:1, maximum:5)]
    public int? StarRating { get; set; }

    /// <summary>
    /// Comment text
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}