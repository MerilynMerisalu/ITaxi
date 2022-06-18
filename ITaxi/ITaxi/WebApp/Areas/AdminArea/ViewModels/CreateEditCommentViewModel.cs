using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCommentViewModel
{
    public Guid Id { get; set; }

    [DisplayName(nameof(Drive))]
    public Guid? DriveId { get; set; }
    //public Drive? Drive { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public SelectList? Drives { get; set; }

    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [DisplayName(nameof(Comment))]
    public string CommentText { get; set; } = default!;
    
    #warning check if using any database method is optimal 
}