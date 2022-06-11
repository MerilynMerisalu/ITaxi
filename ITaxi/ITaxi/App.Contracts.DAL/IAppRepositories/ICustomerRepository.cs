using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICustomerRepository: IEntityRepository<Customer>
{
    // Add Custom methods
}