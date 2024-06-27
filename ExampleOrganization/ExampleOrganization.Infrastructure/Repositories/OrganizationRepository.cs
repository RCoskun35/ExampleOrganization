using ExampleOrganization.Domain.Dtos;
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
    internal sealed class OrganizationRepository : Repository<Organization, ApplicationDbContext>, IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;
        public OrganizationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrganizationResult>> GetAllOrganizationWithParent()
        {
            var organizations = await _context.Set<Organization>().ToListAsync();
            var organizationResults = new List<OrganizationResult>();

            void AddOrganizationToResults(Organization org, int degree, List<int> parents)
            {
                // Listeye tekrar eden eleman eklenmesini önlemek için yeni bir liste oluşturuyoruz
                var currentParents = new List<int>(parents);
                currentParents.Add(org.Id);

                organizationResults.Add(new OrganizationResult
                {
                    Id = organizationResults.Count + 1,
                    OrganizationId = org.Id,
                    Name = org.Name,
                    ParentOrganizationId = org.ParentId,
                    Degree = degree,
                    Parents = currentParents
                });

                foreach (var child in organizations.Where(o => o.ParentId == org.Id))
                {
                    AddOrganizationToResults(child, degree + 1, currentParents);
                }
            }

            foreach (var rootOrganization in organizations.Where(o => o.ParentId == null))
            {
                AddOrganizationToResults(rootOrganization, 0, new List<int>());
            }

            // SubOrganizations alanını güncelle
            foreach (var orgResult in organizationResults)
            {
                var subOrgs = organizationResults
                    .Where(o => o.Parents.Contains(orgResult.OrganizationId))
                    .Select(o => o.OrganizationId).ToList();
                subOrgs.Sort(); 
                orgResult.SubOrganizations = subOrgs;
            }

            return organizationResults;
        }
    }
}
