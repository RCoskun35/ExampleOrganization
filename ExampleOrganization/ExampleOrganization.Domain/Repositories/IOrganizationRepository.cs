using ExampleOrganization.Domain.Abstractions;
using ExampleOrganization.Domain.Entities;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IOrganizationRepository:IRepository<Organization>
    {
        IQueryable<Organization> GetAllOrganizationWithParent();
    }
}
