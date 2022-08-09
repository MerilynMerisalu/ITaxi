
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;


namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCommentViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Driver), Name = "JobTitle")]
    public string DriverName { get; set; } = default!;

    [Display(ResourceType = typeof(Customer), Name = "CustomerName")]
    public string CustomerName { get; set; } = default!;

    [Display(ResourceType = typeof(Comment),
        Name = nameof(Drive))] 
    public string Drive { get; set; } = default!;

    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}