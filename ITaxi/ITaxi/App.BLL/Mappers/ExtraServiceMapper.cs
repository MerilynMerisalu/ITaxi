using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Mappers
{
    public class ExtraServiceMapper : BaseMapper<App.BLL.DTO.AdminArea.ExtraServiceDTO, App.DAL.DTO.AdminArea.ExtraServiceDTO>
    {
        public ExtraServiceMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
