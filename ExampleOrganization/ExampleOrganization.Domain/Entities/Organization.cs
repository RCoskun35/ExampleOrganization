using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name{ get; set; }=string.Empty;
        public int? ParentId { get; set; }
        public Organization? Parent { get; set; }
        public ICollection<Organization> Children { get; set; } = new List<Organization>();
        public ICollection<Organization> AllChildren { get; set; } = new List<Organization>();


    }
}
