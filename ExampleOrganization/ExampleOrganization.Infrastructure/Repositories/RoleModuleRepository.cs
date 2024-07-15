using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Context;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Infrastructure.Repositories
{
    internal class RoleModuleRepository:Repository<RoleModule,ApplicationDbContext>,IRoleModuleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleModuleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
