using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Context;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace ExampleOrganization.Infrastructure.Repositories
{
    internal sealed class RoleMenuRepository : Repository<RoleMenu, ApplicationDbContext>, IRoleMenuRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleMenuRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
