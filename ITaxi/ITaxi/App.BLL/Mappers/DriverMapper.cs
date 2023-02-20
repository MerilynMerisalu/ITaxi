﻿using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DriverMapper: BaseMapper<DTO.AdminArea.DriverDTO, App.DAL.DTO.AdminArea.DriverDTO>
{
    public DriverMapper(IMapper mapper) : base(mapper)
    {
    }
}