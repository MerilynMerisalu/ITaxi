using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using App.DAL.DTO.AdminArea;
using Base.Contracts;

namespace App.DAL.EF.Repositories;

public class VehicleTypeRepository : BaseEntityRepository<VehicleTypeDTO, VehicleType, AppDbContext>, IVehicleTypeRepository
{
    public VehicleTypeRepository(AppDbContext dbContext, IMapper<VehicleTypeDTO, App.Domain.VehicleType> mapper) :
        base(dbContext, mapper)
    {
    }


    public async Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypesOrderedAsync(bool noTracking = true)
    {
#warning: special handling of OrderBy to account for language transalation
        var res = await CreateQuery(noTracking).ToListAsync();
        return res.OrderBy(x => (string)x.VehicleTypeName).ToList().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleTypeDTO> GetAllVehicleTypesOrdered(bool noTracking = true)
    {
#warning: special handling of OrderBy to account for language transalation
        return CreateQuery(noTracking)
            .ToList() // Bring into memory "Materialize"
            .OrderBy(v => v.VehicleTypeName).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypesDTOAsync(bool noTracking = true)
    {

        var vehicleTypeDtos = new List<VehicleTypeDTO>();
        var vehicleTypes = await CreateQuery(noTracking).ToListAsync();
        foreach (var vehicleType in vehicleTypes)
        {
            var vehicleTypeDto = new VehicleTypeDTO()
            {
                Id = vehicleType.Id,
                VehicleTypeName = vehicleType.VehicleTypeName!.Translate("en")!

            };
            vehicleTypeDtos.Add(vehicleTypeDto);
        }

        return vehicleTypeDtos;
    }

    public IEnumerable<VehicleTypeDTO> GetAllVehicleTypesDTO(bool noTracking = true)
    {

        List<VehicleTypeDTO> vehicleTypeDtos = new();
        var vehicleTypes = CreateQuery(noTracking).ToList();
        foreach (var vehicleType in vehicleTypes)
        {
            var vehicleTypeDto = new VehicleTypeDTO()
            {
                Id = vehicleType.Id,
                VehicleTypeName = vehicleType.VehicleTypeName

            };
            vehicleTypeDtos.Add(vehicleTypeDto);
        }

        return vehicleTypeDtos;
    }

    protected override IQueryable<VehicleType> CreateQuery(bool noTracking = true, bool noIncludes = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes);
        if (!noIncludes)
            query = query.Include(t => t.VehicleTypeName)
                         .ThenInclude(t => t.Translations);
        return query;
    }
}