using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CustomerMapper: BaseMapper<DTO.AdminArea.CustomerDTO, Domain.Customer>
{
    public CustomerMapper(IMapper mapper) : base(mapper)
    {
    }
}