using App.Domain;
using App.DTO.AdminArea;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class CityMapper: IMapper<App.DTO.AdminArea.CityDTO, App.Domain.City>
{
    public CityDTO? Map(City? entity)
    {
        throw new NotImplementedException();
    }

    public City? Map(CityDTO? entity)
    {
        throw new NotImplementedException();
    }
}