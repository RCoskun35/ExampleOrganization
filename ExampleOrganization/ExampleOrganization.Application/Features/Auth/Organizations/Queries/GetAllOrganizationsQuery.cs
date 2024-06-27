using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ExampleOrganization.Application.Features.Auth.Organizations.Queries
{
    public sealed record GetAllOrganizationsQuery():IRequest<List<OrganizationResult>>;
    
}
