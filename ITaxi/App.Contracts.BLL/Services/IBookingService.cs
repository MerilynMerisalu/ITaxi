using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBookingService: IEntityService<App.BLL.DTO.AdminArea.BookingDTO>,
    IBookingRepositoryCustom<App.BLL.DTO.AdminArea.BookingDTO>
{
    
}