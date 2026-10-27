using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Contracts.ViewModels;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class AdminDTO : DomainEntityMetaId, IShowHideItem
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

}