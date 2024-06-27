using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Services;
using MediatR;

namespace ExampleOrganization.Application.Features.Auth.Organizations.Queries
{
    public sealed record GetAllOrganizationsQuery():IRequest<List<HierarchyResult>>;
    
}
