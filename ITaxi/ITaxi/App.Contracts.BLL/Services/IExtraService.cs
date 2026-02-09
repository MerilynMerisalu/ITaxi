using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.BLL.Services
{
    public interface IExtraService : IEntityService<App.BLL.DTO.AdminArea.ExtraServiceDTO>, 
        IExtraServiceCustomRepository<App.BLL.DTO.AdminArea.ExtraServiceDTO>
    {

    }
}
