using ExampleOrganization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Dtos
{
    public class RolesAndMenus
    {
        public List<Role> Roles { get; set; }=new List<Role>();
        public List<Menu> Menus { get; set; }= new List<Menu>();
    }
    public class UserAndRole
    {
        public List<AppUser> Users { get; set; } = new List<AppUser>();
        public List<Role> Roles { get; set; } = new List<Role>();
    }

}
