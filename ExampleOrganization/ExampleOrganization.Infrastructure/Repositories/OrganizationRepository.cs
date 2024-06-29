using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Context;
using ExampleOrganization.Infrastructure.Services;
using GenericHierarchy;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace ExampleOrganization.Infrastructure.Repositories
{
    internal sealed class OrganizationRepository : Repository<Organization, ApplicationDbContext>, IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;
        public OrganizationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<HierarchyResult>> GetAllOrganizationWithParent()
        {
            var organizations = await _context.Set<Organization>().ToListAsync();
            var organizationResults = HierarchyService<Organization>.GetHierarchyResults(organizations);
            return organizationResults;
        }
    }
}
