using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BookingMapper: BaseMapper<DTO.AdminArea.BookingDTO, App.DAL.DTO.AdminArea.BookingDTO>
{
    public BookingMapper(IMapper mapper) : base(mapper)
    {
    }
}