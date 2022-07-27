using System.ComponentModel;
using App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCommentViewModel
{
    public Guid Id { get; set; }

    [DisplayName(nameof(Drive))] public string Drive { get; set; } = default!;

    [DisplayName(nameof(Comment))] public string CommentText { get; set; } = default!;
}