using App.BLL.DTO.AdminArea;
using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Mappers
{
    public class EhakUnitTypeMapper : BaseMapper<App.BLL.DTO.AdminArea.EhakUnitTypeDTO, App.DAL.DTO.AdminArea.EhakUnitTypeDTO>
    {
        public EhakUnitTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
