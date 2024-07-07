using ExampleOrganization.Domain.Entities;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ExampleOrganization.Infrastructure.Context
{
    internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, Role, int>, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);
            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });
            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });
            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId});
            });
            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider });
            });

            //builder.Entity<Organization>()
            // .HasOne(o => o.Parent);

            //builder.Ignore<IdentityUserLogin<int>>();
            //builder.Ignore<IdentityRoleClaim<int>>();
            //builder.Ignore<IdentityUserToken<int>>();
            //builder.Ignore<IdentityUserRole<int>>();
            //builder.Ignore<IdentityUserClaim<int>>();
        }
    }
}