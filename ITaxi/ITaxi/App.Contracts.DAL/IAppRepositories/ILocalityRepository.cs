using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.DAL.IAppRepositories
{
    public interface ILocalityRepository : IEntityRepository<LocalityDTO>, IExtraServiceCustomRepository<LocalityDTO>
    {
    }
}
