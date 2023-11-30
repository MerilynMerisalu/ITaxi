using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DriverAndDriverLicenseCategoryMapper: BaseMapper<DTO.AdminArea.DriverAndDriverLicenseCategoryDTO, 
    App.DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO>
{
    public DriverAndDriverLicenseCategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}