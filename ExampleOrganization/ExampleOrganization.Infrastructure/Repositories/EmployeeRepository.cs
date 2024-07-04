using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Context;
using GenericHierarchy;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Infrastructure.Repositories
{
    internal class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IUserOrganizationRepository _userOrganizationRepository;
        private readonly IOrganizationRepository _organizationRepository;
        public EmployeeRepository(ApplicationDbContext context, IUserOrganizationRepository userOrganizationRepository, IOrganizationRepository organizationRepository) : base(context)
        {
            _context = context;
            _userOrganizationRepository = userOrganizationRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<List<Employee>> GetEmployeesToUser(int userId)
        {
            var userOrganizations = _userOrganizationRepository.Where(x => x.UserId == userId).Select(x=>x.OrganizationId).ToList();
            var organizationList = await _organizationRepository.Where(x => userOrganizations.Contains(x.Id)).ToListAsync();
            var hierarchyList= HierarchyService<Organization>.GetHierarchyResults(await _organizationRepository.GetAll().ToListAsync());
            var userHierarchyList = hierarchyList.Where(x=>userOrganizations.Contains(x.EntityId)).ToList();
            var subEntityIds = userHierarchyList.SelectMany(hr => hr.SubEntities).ToList();

            var employees = await _context.Set<Employee>()
                .Include(x => x.Organization)
                .Where(e => subEntityIds.Contains(e.OrganizationId ?? 0))
                .ToListAsync();


            return employees;
        }

        public async Task<List<Employee>> GetFullEmployees()
        {
            
            return await(_context.Set<Employee>().Include(x=>x.Organization).ToListAsync());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
