using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.DAL.EF.Mappers
{
    public class LocalityMapper : BaseMapper<App.DAL.DTO.AdminArea.LocalityDTO, App.Domain.Locality>
    {
        public LocalityMapper(IMapper mapper) : base(mapper)
        {
        }
    }

}
