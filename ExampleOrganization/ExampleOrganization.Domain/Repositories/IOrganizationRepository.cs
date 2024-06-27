using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Services;
using GenericRepository;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IOrganizationRepository:IRepository<Organization>
    {
        Task<List<HierarchyResult>> GetAllOrganizationWithParent();
    }
}
