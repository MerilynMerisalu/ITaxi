using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.CustomerArea.ViewModels;

/// <summary>
/// Create comment view model
/// </summary>
public class CreateCommentViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Drive id
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Comment), Name = nameof(Drive))]
    public Guid? DriveId { get; set; }

    /// <summary>
    /// Drive time and driver
    /// </summary>
    public string? DriveTimeAndDriver { get; set; }

    /// <summary>
    /// Drive
    /// </summary>
    public Drive? Drive { get; set; }

    /// <summary>
    /// List of drives
    /// </summary>
    public SelectList? Drives { get; set; }

    /// <summary>
    /// Comment text
    /// </summary>
    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Comment),
        Name = "CommentName")]
    public string CommentText { get; set; } = default!;
    
    /// <summary>
    /// Rating for the drive
    /// </summary>
    [Range(minimum:0, maximum:5)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "Rating")]
    public int? StarRating { get; set; }
}