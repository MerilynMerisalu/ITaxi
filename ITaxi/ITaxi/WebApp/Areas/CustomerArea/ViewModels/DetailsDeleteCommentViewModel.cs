using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.CustomerArea;

namespace WebApp.Areas.CustomerArea.ViewModels;

public class DetailsDeleteCommentViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Comment), Name = "Driver")]
    public string DriverName { get; set; } = default!;
    
    [Display(ResourceType = typeof(Comment),
        Name = nameof(Drive))] 
    public string Drive { get; set; } = default!;

    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}