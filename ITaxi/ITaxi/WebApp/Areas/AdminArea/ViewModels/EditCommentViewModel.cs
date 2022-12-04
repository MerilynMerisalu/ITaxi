using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditCommentViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public Guid? DriveId { get; set; }

    public string? DriveTimeAndDriver { get; set; }

    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public Drive? Drive { get; set; }


    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Comment),
        Name = "CommentName")]
    public string CommentText { get; set; } = default!;

}