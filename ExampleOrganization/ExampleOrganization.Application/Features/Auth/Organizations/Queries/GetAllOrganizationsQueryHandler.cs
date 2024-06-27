using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using TS.Result;
using ExampleOrganization.Domain.Dtos;

namespace ExampleOrganization.Application.Features.Auth.Organizations.Queries
{
    internal sealed class GetAllOrganizationsQueryHandler(IOrganizationRepository organizationRepository) : IRequestHandler<GetAllOrganizationsQuery, List<OrganizationResult>>
    {
        public async Task<List<OrganizationResult>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var organizations = await organizationRepository.GetAllOrganizationWithParent();
          
            return organizations;
        }

        
    }
   


}