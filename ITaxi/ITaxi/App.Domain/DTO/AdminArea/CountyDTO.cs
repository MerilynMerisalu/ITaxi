using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.DTO.AdminArea;

public class CountyDTO: DomainEntityMetaId
{
    [Required()]
    [MaxLength(50)]
    public string CountyName { get; set; } = default!;

    public int NumberOfCities { get; set; }
}