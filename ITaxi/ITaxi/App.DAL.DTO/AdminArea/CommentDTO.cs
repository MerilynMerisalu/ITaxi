using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO.AdminArea;

public class CommentDTO: DomainEntityMetaId
{
    
   
    public Guid? DriveId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public DriveDTO? Drive { get; set; }


    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "CommentName")]
    public string? CommentText { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "Rating")]
    [Range(minimum:0, maximum:5)]
    public int? StarRating { get; set; }

}