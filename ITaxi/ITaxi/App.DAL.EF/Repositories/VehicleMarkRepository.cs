using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleMarkRepository : BaseEntityRepository<VehicleMarkDTO,App.Domain.VehicleMark, AppDbContext>, IVehicleMarkRepository
{
    public VehicleMarkRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.VehicleMarkDTO, App.Domain.VehicleMark> mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<VehicleMarkDTO>> GetAllVehicleMarkOrderedAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking)
            .OrderBy(v => v.VehicleMarkName).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleMarkDTO> GetAllVehicleMarkOrdered(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(v => v.VehicleMarkName).ToList()
            .Select(e => Mapper.Map(e))!;
    }
}