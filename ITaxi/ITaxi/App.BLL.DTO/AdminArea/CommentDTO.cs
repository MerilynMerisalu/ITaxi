using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class CommentDTO: DomainEntityMetaId
{
    
    public Guid? DriveId { get; set; }

    /// <summary>
    /// Description of the Drive, to show the Customer
    /// It should be the formatted drive time.
    /// </summary>
    
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public string DriveCustomerStr { get; set; } = default!;

    

    /// <summary>
    /// Description of the Driver 
    /// </summary>
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Driver")]
    public string DriverName { get; set; } = default!;
    
    /// <summary>
    /// Description of the Customer 
    /// </summary>
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Customer")]

    public string CustomerName { get; set; } = default!;
    
    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "CommentName")]
    public string? CommentText { get; set; }
   
    /// <summary>
    /// Rating for the drive
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "Rating")]
    public int? StarRating { get; set; }
}