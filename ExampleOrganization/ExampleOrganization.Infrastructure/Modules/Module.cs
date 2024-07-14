using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Infrastructure.Modules
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
    }
}
