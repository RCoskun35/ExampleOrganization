using ExampleOrganization.Application.Features.Auth.Login;
using ExampleOrganization.Application.Services;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ExampleOrganization.Infrastructure.Services
{
    internal class JwtProvider(
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> jwtOptions
        //IRoleMenuRepository roleMenuRepository,
        //IMenuRepository menuRepository,
        //RoleManager<Role> roleManager
        ) : IJwtProvider
    {
        public async Task<LoginCommandResponse> CreateToken(AppUser user)
        {
            List<Claim> claims = new()
            {
                //new Claim("menu", (-1).ToString()),
                //new Claim("menu", (-2).ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Name", user.FullName),
                new Claim("Email", user.Email ?? ""),
                new Claim("UserName", user.UserName ?? "")
            };
            //var userRoleNames = await userManager.GetRolesAsync(user);
            //var userRoles = await roleManager.Roles.Where(x => userRoleNames.Contains(x.Name ?? "")).ToListAsync();
            //var roleMenus = roleMenuRepository.GetAll().Where(x => userRoles.Select(y=>y.Id).ToList().Contains(x.RoleId)).Select(x=>x.MenuId).ToList();
            //var menus = menuRepository.GetAll().Where(x=>roleMenus.Contains(x.Id)).Select(y=>y.Id).ToList();
            //claims.AddRange(menus.Select(x => new Claim("menu", x.ToString())));

            DateTime expires = DateTime.UtcNow.AddMonths(1);


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));

            JwtSecurityToken jwtSecurityToken = new(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512));

            JwtSecurityTokenHandler handler = new();

            string token = handler.WriteToken(jwtSecurityToken);

            string refreshToken = Guid.NewGuid().ToString();
            DateTime refreshTokenExpires = expires.AddHours(1);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = refreshTokenExpires;

            await userManager.UpdateAsync(user);

            return new(token, refreshToken, refreshTokenExpires);
        }
    }
}
