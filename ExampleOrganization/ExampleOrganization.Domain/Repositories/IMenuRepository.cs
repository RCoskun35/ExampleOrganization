using ExampleOrganization.Domain.Entities;
using GenericRepository;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IMenuRepository:IRepository<Menu>,IUnitOfWork
    {
    }
}
