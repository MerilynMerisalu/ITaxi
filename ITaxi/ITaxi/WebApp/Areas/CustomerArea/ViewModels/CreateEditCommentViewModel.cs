using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.CustomerArea.ViewModels;

public class CreateEditCommentViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Comment), Name = nameof(Drive))]
    public Guid? DriveId { get; set; }
    public Drive? Drive { get; set; }

    public SelectList? Drives { get; set; }

    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Comment),
        Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}
