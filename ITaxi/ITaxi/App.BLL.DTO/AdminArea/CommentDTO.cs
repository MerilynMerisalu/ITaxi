using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class CommentDTO: DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
    public Guid? DriveId { get; set; }

    /// <summary>
    /// Description of the Drive, to show the Customer
    /// It should be the formatted drive time.
    /// </summary>
    public string DriveCustomerStr { get; set; } = default!;

    public string DriveTimeAndDriver { get; set; } = default!;

    /// <summary>
    /// Description of the Driver 
    /// </summary>
    public string DriverName { get; set; } = default!;
    
    /// <summary>
    /// Description of the Customer 
    /// </summary>
    public string CustomerName { get; set; } = default!;
    
    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "CommentName")]
    public string? CommentText { get; set; }
}