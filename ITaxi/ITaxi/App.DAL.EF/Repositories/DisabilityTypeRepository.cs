﻿using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DisabilityTypeRepository : BaseEntityRepository<DisabilityTypeDTO, App.Domain.DisabilityType, AppDbContext>,
    IDisabilityTypeRepository
{
    public DisabilityTypeRepository(AppDbContext dbContext, IMapper<DAL.DTO.AdminArea.DisabilityTypeDTO,
        App.Domain.DisabilityType> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DisabilityTypeDTO>> GetAllDisabilityTypeDtoAsync(string? roleName = null, bool noTracking = true)
    {
        List<DisabilityTypeDTO> disabilityTypeDtoList = new();
        var disabilityTypes = await CreateQuery().ToListAsync();
        foreach (var disabilityType in disabilityTypes)
        {
            var disabilityTypeDto = new DisabilityTypeDTO()
            {
                Id = disabilityType.Id,
                DisabilityTypeName = disabilityType.DisabilityTypeName
            };
            disabilityTypeDtoList.Add(disabilityTypeDto);
        }

        return disabilityTypeDtoList;
    }

    public IEnumerable<DisabilityTypeDTO> GetAllDisabilityTypeDto(string? roleName = null, bool noTracking = true)
    {
        List<DisabilityTypeDTO> disabilityTypeDtoList = new();
        var disabilityTypes = CreateQuery().ToList();
        foreach (var disabilityType in disabilityTypes)
        {
            var disabilityTypeDto = new DisabilityTypeDTO()
            {
                Id = disabilityType.Id,
                DisabilityTypeName = disabilityType.DisabilityTypeName
            };
            disabilityTypeDtoList.Add(disabilityTypeDto);
        }

        return disabilityTypeDtoList;
    }

    public async Task<IEnumerable<DisabilityTypeDTO>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking)
                .OrderBy(d => d.DisabilityTypeName).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DisabilityTypeDTO> GetAllOrderedDisabilityTypes(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(d => d.DisabilityTypeName)
                .ToList().Select(e => Mapper.Map(e))!
            ;
    }

    public async Task<bool> HasAnyCustomersAsync(Guid disabilityTypeId)
    {
        return await RepoDbContext.Customers.AnyAsync(d => d.DisabilityTypeId.Equals(disabilityTypeId));
    }

    public bool HasAnyCustomers(Guid disabilityTypeId)
    {
        return RepoDbContext.Customers.Any(d => d.DisabilityTypeId.Equals(disabilityTypeId));
    }

    protected override IQueryable<DisabilityType> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes, showDeleted);
        if (noTracking) query = query.AsNoTracking();
        if (!noIncludes)
            query = query.Include(c => c.DisabilityTypeName)
                .ThenInclude(c => c.Translations);
        return query;
    }
}