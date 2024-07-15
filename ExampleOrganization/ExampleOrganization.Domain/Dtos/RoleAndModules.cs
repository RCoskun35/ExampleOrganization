using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Dtos
{
    public class RoleAndModules
    {
        public int RoleId { get; set; }
        public List<int> ModuleIds { get; set; }= new List<int>();
    }
    public class UserAndRoles
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}
