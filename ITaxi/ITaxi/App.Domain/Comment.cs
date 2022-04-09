using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Comment: DomainEntityMetaId
{
    public Guid? DriveId { get; set; }

    public Drive? Drive { get; set; }

    
    

        [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [DisplayName("Comment")]
    public string? CommentText { get; set; }
}