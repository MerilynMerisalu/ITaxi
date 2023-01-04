using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriverAndDriverLicenseCategoryMapper: BaseMapper<App.DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO, 
    App.Domain.DriverAndDriverLicenseCategory>
{
    public DriverAndDriverLicenseCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}