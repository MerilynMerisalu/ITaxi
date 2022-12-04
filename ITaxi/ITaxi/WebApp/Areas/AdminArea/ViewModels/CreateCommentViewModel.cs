using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Comment = App.Resources.Areas.App.Domain.AdminArea.Comment;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateCommentViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public Guid? DriveId { get; set; }

    public Drive? Drive { get; set; }

    public SelectList? Drives { get; set; }

    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Comment),
        Name = "CommentName")]
    public string CommentText { get; set; } = default!;
    
    public string? DriveTimeAndDriver { get; set; }
}