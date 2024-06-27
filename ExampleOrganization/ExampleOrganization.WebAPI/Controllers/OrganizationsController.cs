using ExampleOrganization.Application.Features.Auth.Organizations.Queries;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Domain.Services;
using ExampleOrganization.WebAPI.Abstractions;
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
        public OrganizationsController(IMediator mediator, IOrganizationRepository organizationRepository) : base(mediator)
        {
            _organizationRepository = organizationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Test()
        {
            var list =await _organizationRepository.GetAll().ToListAsync();
            return Ok(HierarchyService<Organization>.GetHierarchyResults(list));
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
