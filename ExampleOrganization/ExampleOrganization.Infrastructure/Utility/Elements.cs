using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Infrastructure.Utility
{
    public static class Elements
    {
        public static List<RelatedOrganization> GetElements(List<int> parents, List<Organization> list)
        {
            List<RelatedOrganization> result = new List<RelatedOrganization>();

            foreach (int parentId in parents)
            {
                Organization? parentOrg = list.FirstOrDefault(o => o.Id == parentId);
                if (parentOrg != null)
                {
                    result.Add(new RelatedOrganization
                    {
                        Id = parentOrg.Id,
                        Name = parentOrg.Name
                    });
                }
            }

            return result;
        }
    }
}
