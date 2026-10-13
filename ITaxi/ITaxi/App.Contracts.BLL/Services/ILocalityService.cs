using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.BLL.Services
{
    public interface ILocalityService : IEntityService<App.BLL.DTO.AdminArea.LocalityDTO>,
    ILocalityRepositoryCustom<App.BLL.DTO.AdminArea.LocalityDTO> // Add custom stuff
    {
    }

}
