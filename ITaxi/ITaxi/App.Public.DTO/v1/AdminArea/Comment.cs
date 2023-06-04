using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.AdminArea;
using Base.Domain;

namespace App.Public.DTO.v1.AdminArea;

public class Comment : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public Guid? DriveId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public Drive? Drive { get; set; }

    // this is mapping, so it should be in the automapper!
    // I CAN NOT use readonly properties, not without making the relationships very inefficient
    public string DriveCustomerStr { get; set; } = default!;

    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "CommentName")]
    public string? CommentText { get; set; }
}