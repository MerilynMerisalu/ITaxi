using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CustomerMapper: BaseMapper<DTO.AdminArea.CustomerDTO, App.DAL.DTO.AdminArea.CustomerDTO>
{
    public CustomerMapper(IMapper mapper) : base(mapper)
    {
    }
}