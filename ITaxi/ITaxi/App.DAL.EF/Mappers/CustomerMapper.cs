using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CustomerMapper: BaseMapper<App.DAL.DTO.AdminArea.CustomerDTO, App.Domain.Customer>
{
    public CustomerMapper(IMapper mapper) : base(mapper)
    {
    }
}