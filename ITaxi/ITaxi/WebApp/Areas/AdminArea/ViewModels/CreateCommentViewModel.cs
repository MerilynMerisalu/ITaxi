using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Comment = App.Resources.Areas.App.Domain.AdminArea.Comment;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create comment view model
/// </summary>
public class CreateCommentViewModel
{
    /// <summary>
    /// Comment id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Comment drive id
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public Guid? DriveId { get; set; }

    /// <summary>
    /// Comment drive
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
    [Display(ResourceType = typeof(Comment),
        Name = "CommentName")]
    public string CommentText { get; set; } = default!;
    
    /// <summary>
    /// Drive time and driver
    /// </summary>
    public string? DriveTimeAndDriver { get; set; }

   
    
    /// <summary>
    /// Rating for the drive
    /// </summary>
    [Range(minimum:0, maximum:5)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "Rating")]
    public int? StarRating { get; set; }
}