using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.BLL.Services
{
    public interface ILocalityService : IEntityService<App.BLL.DTO.AdminArea.LocalityDTO>,
      ILocalityCustomRepository<App.BLL.DTO.AdminArea.LocalityDTO>
    {

    }

}
