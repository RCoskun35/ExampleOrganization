using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using GenericHierarchy;
using GenericRepository;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IOrganizationRepository:IRepository<Organization>
    {
        Task<List<HierarchyResult>> GetAllOrganizationWithParent();
    }
}
