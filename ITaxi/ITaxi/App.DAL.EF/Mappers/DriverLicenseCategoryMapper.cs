using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriverLicenseCategoryMapper:BaseMapper<DTO.AdminArea.DriverLicenseCategoryDTO, 
    Domain.DriverLicenseCategory>
{
    public DriverLicenseCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}