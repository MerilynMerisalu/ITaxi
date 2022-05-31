using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Base.Domain;

namespace App.DAL.EF.Repositories;

public class BookingRepository: BaseEntityRepository<Booking, AppDbContext>, IBookingRepository
{
    public BookingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}