using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BookingMapper: BaseMapper<App.BLL.DTO.AdminArea.BookingDTO, App.DAL.DTO.AdminArea.BookingDTO>
{
    public BookingMapper(IMapper mapper) : base(mapper)
    {
    }
}