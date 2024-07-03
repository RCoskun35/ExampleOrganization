using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using GenericHierarchy;
using GenericRepository;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IOrganizationRepository:IRepository<Organization>, IUnitOfWork
    {
        Task<List<HierarchyResult>> GetAllOrganizationWithParent();
        Task<bool> AddEmployee(List<Employee> employees);
        Task<bool> AddUserOrganization(List<UserOrganization> userOrganizations);
    }
}
