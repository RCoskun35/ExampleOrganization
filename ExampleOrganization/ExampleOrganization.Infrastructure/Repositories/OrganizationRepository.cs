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

            var organizationDict = organizations.ToDictionary(o => o.Id);

            void AddOrganizationToResults(Organization org, int degree, string parents)
            {
                organizationResults.Add(new OrganizationResult
                {
                    Id = organizationResults.Count + 1,
                    OrganizationId = org.Id,
                    Name = org.Name,
                    ParentOrganizationId = org.ParentId,
                    Degree = degree,
                    Parents = parents,
                    SubOrganizations = ""
                });

                foreach (var child in organizations.Where(o => o.ParentId == org.Id))
                {
                    AddOrganizationToResults(child, degree + 1, parents + "," + child.Id);
                }
            }

            foreach (var rootOrganization in organizations.Where(o => o.ParentId == null))
            {
                AddOrganizationToResults(rootOrganization, 0, rootOrganization.Id.ToString());
            }

            // SubOrganizations alanını güncelle
            foreach (var orgResult in organizationResults)
            {
                var subOrgs = organizationResults
                    .Where(o => o.Parents.Contains("," + orgResult.OrganizationId + ",") || o.Parents.StartsWith(orgResult.OrganizationId + ",") || o.Parents.EndsWith("," + orgResult.OrganizationId) || o.Parents == orgResult.OrganizationId.ToString())
                    .Select(o => o.OrganizationId.ToString());
                orgResult.SubOrganizations = string.Join(",", subOrgs);
            }

            return organizationResults;
        }
    }
}
