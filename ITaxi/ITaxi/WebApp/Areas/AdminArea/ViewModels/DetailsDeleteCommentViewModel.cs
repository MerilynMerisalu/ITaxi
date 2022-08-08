
using System.ComponentModel.DataAnnotations;


namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCommentViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment),
        Name = nameof(Drive))] public string Drive { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}