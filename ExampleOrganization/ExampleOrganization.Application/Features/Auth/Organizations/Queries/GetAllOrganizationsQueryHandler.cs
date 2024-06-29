using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Repositories;
using GenericHierarchy;
using MediatR;

namespace ExampleOrganization.Application.Features.Auth.Organizations.Queries
{
    internal sealed class GetAllOrganizationsQueryHandler(IOrganizationRepository organizationRepository) : IRequestHandler<GetAllOrganizationsQuery, List<HierarchyResult>>
    {
        public async Task<List<HierarchyResult>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var organizations = await organizationRepository.GetAllOrganizationWithParent();
          
            return organizations;
        }

        
    }
   


}