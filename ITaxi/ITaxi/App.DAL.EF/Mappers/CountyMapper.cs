using App.Domain;
using App.DTO.AdminArea;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class CountyMapper:IMapper<CountyDTO, County>
{
    public CountyDTO? Map(County? entity)
    {
        throw new NotImplementedException();
    }

    public County? Map(CountyDTO? entity)
    {
        throw new NotImplementedException();
    }
}