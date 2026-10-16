using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Mappers
{
    public class LocalityMapper : BaseMapper<App.BLL.DTO.AdminArea.LocalityDTO, App.DAL.DTO.AdminArea.LocalityDTO>
    {
        public LocalityMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
