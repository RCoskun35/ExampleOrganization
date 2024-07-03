using ExampleOrganization.Application.Features.Auth.Organizations.Queries;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.WebAPI.Abstractions;
using GenericHierarchy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
                Id = x.Id,
                EntryId=x.EntityId,
                Name = x.Name,
                ParentEntityId = x.ParentEntityId,
                Degree = x.Degree,
                Parents = GetElements(x.Parents,list),
                SubEntities = GetElements(x.SubEntities,list),
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
                //var result = await _organizationRepository.AddUserOrganization(userOrganizations);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           

        }






        private List<RelatedOrganization> GetElements(List<int> parents, List<Organization> list)
        {
            List<RelatedOrganization> result = new List<RelatedOrganization>();

            foreach (int parentId in parents)
            {
                Organization? parentOrg = list.FirstOrDefault(o => o.Id == parentId);
                if (parentOrg != null)
                {
                    result.Add(new RelatedOrganization
                    {
                        Id = parentOrg.Id,
                        Name = parentOrg.Name
                    });
                }
            }

            return result;
        }

        public class OrganizationDto
        {
            public int Id { get; set; }
            public int EntryId { get; set; }
            public int? ParentEntityId { get; set; }
            public int Degree{ get; set; }
            public string Name { get; set; } = string.Empty;
            public List<RelatedOrganization> Parents { get; set; }=new List<RelatedOrganization>();
            public List<RelatedOrganization> SubEntities { get; set; }=new List<RelatedOrganization>();
        }
        public class RelatedOrganization
        {
            public int Id { get; set; }
            public string Name { get; set; }=string.Empty;
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
