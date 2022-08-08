using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Comment : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment), 
        Name = "Drive" )]
    public Guid? DriveId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment), 
        Name = "Drive" )]
    public Drive? Drive { get; set; }


    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment), 
        Name = "CommentName" )]
    public string? CommentText { get; set; }
}