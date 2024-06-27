using ExampleOrganization.Application.Features.Auth.Organizations.Queries;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ExampleOrganization.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizationsController : ApiController
    {
        public OrganizationsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve
            //};

            //var json = JsonSerializer.Serialize(response, options);
            return Ok(response);
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
