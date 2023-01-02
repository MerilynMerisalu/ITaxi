using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriverLicenseCategoryMapper:BaseMapper<App.DAL.DTO.AdminArea.DriverLicenseCategoryDTO, App.Domain.DriverLicenseCategory>
{
    public DriverLicenseCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}