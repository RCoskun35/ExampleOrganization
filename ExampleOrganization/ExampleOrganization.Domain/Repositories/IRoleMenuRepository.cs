using ExampleOrganization.Domain.Entities;
using GenericRepository;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IRoleMenuRepository:IRepository<RoleMenu>,IUnitOfWork
    {
    }
}
