using ExampleOrganization.Application.Features.Auth.Login;
using ExampleOrganization.Domain.Entities;

namespace ExampleOrganization.Application.Services
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateToken(AppUser user);
    }
}
