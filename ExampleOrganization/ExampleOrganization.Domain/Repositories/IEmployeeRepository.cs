using ExampleOrganization.Domain.Entities;
using GenericRepository;

namespace ExampleOrganization.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>,IUnitOfWork
    {
        Task<List<Employee>> GetFullEmployees();
        Task<List<Employee>> GetEmployeesToUser(int userId);
    }
    

}
