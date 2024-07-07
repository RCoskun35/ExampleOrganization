using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Dtos
{
    public class RelatedOrganization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class RelatedMenu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
