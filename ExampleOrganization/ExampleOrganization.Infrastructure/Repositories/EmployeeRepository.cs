using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Context;
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
        public EmployeeRepository(ApplicationDbContext context, IUserOrganizationRepository userOrganizationRepository) : base(context)
        {
            _context = context;
            _userOrganizationRepository = userOrganizationRepository;
        }

        public async Task<List<Employee>> GetEmployeesToUser(int userId)
        {
            var userOrganizations = _userOrganizationRepository.Where(x => x.UserId == userId).Select(x=>x.OrganizationId).ToList();
            var employees =await  _context.Set<Employee>().Include(x => x.Organization).Where(x=>userOrganizations.Contains(x.OrganizationId ?? 0)).ToListAsync(); 


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
