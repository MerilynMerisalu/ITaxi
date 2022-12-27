
using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;

using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL:BaseBLL<IAppUnitOfWork>, IAppBLL
{
    private readonly AutoMapper.IMapper _mapper;
    protected IAppUnitOfWork UnitOfWork;
    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public override async Task<int> SaveChangesAsync()
    {
        return await UnitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }

    public ICountyService Counties => _counties ?? (ICountyService) new CountyService(UnitOfWork.Counties, new CountyMapper(_mapper));
    public ICityService Cities => _cities ?? (ICityService) new CityService(UnitOfWork.Cities, new CityMapper(_mapper));
    
    private ICountyService? _counties;
    private ICityService? _cities;
}