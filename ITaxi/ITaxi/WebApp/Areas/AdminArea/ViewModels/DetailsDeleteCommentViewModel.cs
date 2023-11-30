using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete comment view model
/// </summary>
public class DetailsDeleteCommentViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Comment id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver name
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "JobTitle")]
    public string DriverName { get; set; } = default!;

    /// <summary>
    /// Customer name
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = "CustomerName")]
    public string CustomerName { get; set; } = default!;

    /// <summary>
    /// Drive
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public string Drive { get; set; } = default!;

    /// <summary>
    /// Comment text
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}