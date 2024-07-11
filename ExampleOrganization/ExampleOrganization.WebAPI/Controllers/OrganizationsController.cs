using ExampleOrganization.Application.Features.Auth.Organizations.Queries;
using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Utility;
using ExampleOrganization.WebAPI.Abstractions;
using GenericHierarchy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleOrganization.WebAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizationsController : ApiController
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserOrganizationRepository _userOrganizationRepository;
        
        private readonly IHttpContextAccessor _contextAccessor;
        public OrganizationsController(IMediator mediator, IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository, IUserOrganizationRepository userOrganizationRepository, IHttpContextAccessor contextAccessor) : base(mediator)
        {
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _userOrganizationRepository = userOrganizationRepository;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
      
        [HttpPost]
        public async Task<IActionResult> GetOrganizationsWithParentsAndChilds()
        {
            var list =await _organizationRepository.GetAll().ToListAsync();
            var infoList = HierarchyService<Organization>.GetHierarchyResults(list);

            
            var result = infoList.Select(x => new OrganizationDto
            {
                Id = x.EntityId,
                EntryId=x.EntityId,
                Name = x.Name,
                ParentEntityId = x.ParentEntityId,
                Degree = x.Degree,
                Parents =Elements.GetElements(x.Parents,list),
                SubEntities =Elements.GetElements(x.SubEntities,list),
            });

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee()
        {
            var employeeList = new List<Employee>();
            int count = 2;

            for (int i = 0; i < 5000; i++)
            {
                if (count > 25)
                {
                    count = 2;
                }
                employeeList.Add(new Employee
                {
                    Name = "Name" + i.ToString(),
                    Email = "Email" + i.ToString(),
                    Surname = "Surname" + i.ToString(),
                    OrganizationId = count,
                });
                count++;
            }
             await _employeeRepository.AddRangeAsync(employeeList);
            await _employeeRepository.SaveChangesAsync();
            
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> GetFullEmployees()
        {
            return Ok(await _employeeRepository.GetFullEmployees());
        }
        [HttpPost]
        public async Task<IActionResult> GetEmployeesToUser()
        {
            var userId =Convert.ToInt32(_contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value.ToString());
            return Ok(await _employeeRepository.GetEmployeesToUser(userId));
        }
        [HttpPost]
        public async Task<IActionResult> AddUserOrganizations()
        {
            try
            {
                var userOrganizations = new List<UserOrganization>();
                int count = 2;
                var userOrganizationList = new List<UserOrganization>();

                for (int i = 0; i < 25; i++)
                {
                    if (count > 25)
                    {
                        count = 2;
                    }
                    userOrganizations.Add(new UserOrganization
                    {
                        OrganizationId = count+1,
                        UserId = count,
                    });
                    count++;
                }
                await _userOrganizationRepository.AddRangeAsync(userOrganizations);
                await _userOrganizationRepository.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           

        }
        [HttpPost]
        public async Task<IActionResult> AddOrganizationToUser(List<Organization> organizations)
        {
            try
            {
                var userId = Convert.ToInt32(_contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value.ToString());
                var deleteList = _userOrganizationRepository.Where(x => x.UserId == userId).ToList();
                if (deleteList.Count>0)
                {
                     _userOrganizationRepository.DeleteRange(deleteList);
                }
                var orgList = organizations.Select(x => new UserOrganization
                {
                    UserId = userId,
                    OrganizationId = x.Id
                }).ToList();
                await _userOrganizationRepository.AddRangeAsync(orgList);
                await _userOrganizationRepository.SaveChangesAsync();
                return Ok(new Organization());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
          
        }

        
        
        
        
        [HttpPost]
        public IActionResult Test()
        {
            return Ok();
        }



         



        























        //[HttpPost]
        //public async Task<IActionResult> Create(CreateCompanyCommand request, CancellationToken cancellationToken)
        //{
        //    var response = await _mediator.Send(request, cancellationToken);
        //    return StatusCode(response.StatusCode, response);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(UpdateCompanyCommand request, CancellationToken cancellationToken)
        //{
        //    var response = await _mediator.Send(request, cancellationToken);
        //    return StatusCode(response.StatusCode, response);
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteById(DeleteCompanyByIdCommand request, CancellationToken cancellationToken)
        //{
        //    var response = await _mediator.Send(request, cancellationToken);
        //    return StatusCode(response.StatusCode, response);
        //}

        //[HttpPost]
        //public async Task<IActionResult> MigrateAll(MigrateAllCompaniesCommand request, CancellationToken cancellationToken)
        //{
        //    var response = await _mediator.Send(request, cancellationToken);
        //    return StatusCode(response.StatusCode, response);
        //}
    }
}
