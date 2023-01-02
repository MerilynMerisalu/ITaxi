using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DriverLicenseCategoryMapper:BaseMapper<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO, 
    App.DAL.DTO.AdminArea.DriverLicenseCategoryDTO>
{
    public DriverLicenseCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}