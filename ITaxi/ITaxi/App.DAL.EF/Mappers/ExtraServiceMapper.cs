using App.DAL.DTO.AdminArea;
using App.Domain;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.DAL.EF.Mappers
{
    public class ExtraServiceMapper : BaseMapper<ExtraServiceDTO, ExtraService>
    {
        public ExtraServiceMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
