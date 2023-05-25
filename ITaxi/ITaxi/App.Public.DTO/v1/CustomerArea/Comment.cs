using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.AdminArea;
using Base.Domain;

namespace App.Public.DTO.v1.CustomerArea;

public class Comment : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public Guid? DriveId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public DriveDTO? Drive { get; set; }

    public string DriveCustomerStr => $"{Drive!.Booking!.PickUpDateAndTime:g}";

    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "CommentName")]
    public string? CommentText { get; set; }
}