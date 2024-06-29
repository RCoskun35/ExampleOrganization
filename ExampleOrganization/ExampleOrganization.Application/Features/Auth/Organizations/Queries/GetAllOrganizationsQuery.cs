using ExampleOrganization.Domain.Dtos;
using GenericHierarchy;
using MediatR;

namespace ExampleOrganization.Application.Features.Auth.Organizations.Queries
{
    public sealed record GetAllOrganizationsQuery():IRequest<List<HierarchyResult>>;
    
}
