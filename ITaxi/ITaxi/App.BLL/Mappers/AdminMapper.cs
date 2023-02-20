﻿using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class AdminMapper: BaseMapper<DTO.AdminArea.AdminDTO, App.DAL.DTO.AdminArea.AdminDTO>
{
    public AdminMapper(IMapper mapper) : base(mapper)
    {
    }
}