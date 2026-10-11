using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.BLL.Services
{
    public interface IEhakUnitTypeService : IEntityService<App.BLL.DTO.AdminArea.EhakUnitTypeDTO>, IEhakUnitTypeCustomRepository<App.BLL.DTO.AdminArea.EhakUnitTypeDTO>
    {
    }
}
