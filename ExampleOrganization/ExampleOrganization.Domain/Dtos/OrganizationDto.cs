using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Dtos
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int? ParentEntityId { get; set; }
        public int Degree { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RelatedOrganization> Parents { get; set; } = new List<RelatedOrganization>();
        public List<RelatedOrganization> SubEntities { get; set; } = new List<RelatedOrganization>();
    }
    public class MenuDto
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int? ParentEntityId { get; set; }
        public int Degree { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RelatedMenu> Parents { get; set; } = new List<RelatedMenu>();
        public List<RelatedMenu> SubEntities { get; set; } = new List<RelatedMenu>();
    }
}
