
using Base.Domain;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.EF
{
    public class Person : DomainEntityId
    {
        [Required]
        [MaxLength(64)]
        [StringLength(64, MinimumLength = 1)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MaxLength(64)]
        [StringLength(64, MinimumLength = 1)]
        public string LastName { get; set; } = default!;

        public string FirstAndLastName => $"{FirstName} {LastName}"; 
    }
}
