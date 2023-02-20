﻿using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DisabilityTypeMapper: BaseMapper<DTO.AdminArea.DisabilityTypeDTO, App.DAL.DTO.AdminArea.DisabilityTypeDTO>
{
    public DisabilityTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}