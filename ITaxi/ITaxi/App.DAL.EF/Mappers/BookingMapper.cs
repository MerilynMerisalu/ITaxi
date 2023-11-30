using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BookingMapper: BaseMapper<DTO.AdminArea.BookingDTO, Domain.Booking>
{
    public BookingMapper(IMapper mapper) : base(mapper)
    {
    }
}