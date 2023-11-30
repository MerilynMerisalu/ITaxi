using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriverAndDriverLicenseCategoryMapper: BaseMapper<DTO.AdminArea.DriverAndDriverLicenseCategoryDTO, 
    Domain.DriverAndDriverLicenseCategory>
{
    public DriverAndDriverLicenseCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}