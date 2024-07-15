using ExampleOrganization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Dtos
{
    public class UserAndModules
    {
        public AppUser User { get; set; } = new AppUser();
        public List<string> Modules { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();

        public List<string> Parents { get; set; } = new List<string>();


    }
}
