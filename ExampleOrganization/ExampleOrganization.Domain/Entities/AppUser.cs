using Microsoft.AspNetCore.Identity;

namespace ExampleOrganization.Domain.Entities
{
    public sealed class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => string.Join(" ", FirstName, LastName);
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
        public int OrganizationId { get; set; }
    }
}
