using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Dtos
{
    public class OrganizationResult
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentOrganizationId { get; set; }
        public int Degree { get; set; }
        public string Parents { get; set; } = string.Empty;
        public string SubOrganizations { get; set; } = string.Empty;
    }
}
