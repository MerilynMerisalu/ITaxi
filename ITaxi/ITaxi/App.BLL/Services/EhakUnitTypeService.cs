using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.DAL.EF.Repositories;
using Base.BLL;
using Base.Contracts.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Services
{

    public class EhakUnitTypeService : BaseEntityService<App.BLL.DTO.AdminArea.EhakUnitTypeDTO, DAL.DTO.AdminArea.EhakUnitTypeDTO,
    IEhakUnitTypeRepository>, IEhakUnitTypeService
    {
        public EhakUnitTypeService(IEhakUnitTypeRepository repository, IMapper<DTO.AdminArea.EhakUnitTypeDTO, DAL.DTO.AdminArea.EhakUnitTypeDTO> mapper) : base(repository, mapper)
        {
        }
    }
}
