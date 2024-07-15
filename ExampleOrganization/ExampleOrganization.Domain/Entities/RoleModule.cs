using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Entities
{
    public class RoleModule
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
    }
}
