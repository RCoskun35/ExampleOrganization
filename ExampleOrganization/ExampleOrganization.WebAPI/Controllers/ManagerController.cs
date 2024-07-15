using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Utility;
using ExampleOrganization.WebAPI.Abstractions;
using GenericHierarchy;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech.Transcription;
using Microsoft.EntityFrameworkCore;

namespace ExampleOrganization.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagerController : ApiController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ManagerController(IMediator mediator, IMenuRepository menuRepository, IRoleMenuRepository roleMenuRepository, RoleManager<Role> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(mediator)
        {
            _menuRepository = menuRepository;
            _roleMenuRepository = roleMenuRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> GetRoles()
        {
            
            return Ok(await _roleManager.Roles.ToListAsync());  
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role role)
        {
            await _roleManager.CreateAsync(role);
            return Ok(new List<Role>());

        }
        [HttpPost]
        public async Task<IActionResult> GetMenus()
        {
            return Ok(await _menuRepository.GetAll().ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddMenu(Menu menu)
        {
            await _menuRepository.AddAsync(menu);
            await _menuRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> GetRoleMenus()
        {
            var roles =await  _roleManager.Roles.ToListAsync();
            var menus = await _menuRepository.GetAll().ToListAsync();
            var roleMenu = await _roleMenuRepository.GetAll().ToListAsync();
            var result = roleMenu.Select(x => new
            {
                x.Id,
                RoleName = roles.Where(y => y.Id == x.RoleId)?.FirstOrDefault()?.Name ?? "",
                MenuName = menus.Where(y => y.Id == x.MenuId)?.FirstOrDefault()?.Name ?? "",
            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleMenu(RolesAndMenus rolesAndMenus)
        {
            var roleMenus = new List<RoleMenu>();
            foreach (var role in rolesAndMenus.Roles)
            {
                foreach (var menu in rolesAndMenus.Menus)
                {
                    roleMenus.Add(new RoleMenu { MenuId=menu.Id,RoleId=role.Id});
                }
            }
            await _roleMenuRepository.AddRangeAsync(roleMenus);
            await _roleMenuRepository.SaveChangesAsync();
            return Ok(new RoleMenu());
        }


        [HttpPost]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userResult = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userResult.Add(new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Roles = roles.Select(x => x).ToList()
                }) ;
                
            }
            return Ok(userResult);
        }
        [NonAction]
        public async Task RemoveAllRolesFromUserAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var result = await _userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddUserRole(UserAndRole userAndRole)
        {
            var userIdList = userAndRole.Users.Select(a => a.Id).ToList();
            var users = _userManager.Users.Where(x => userIdList.Contains(x.Id)).ToList();
            foreach (var user in users)
            {
                try
                {
                    var roleNames = userAndRole.Roles.Select(x => x.Name ?? "").ToList();
                    await RemoveAllRolesFromUserAsync(user);
                    var result = await _userManager.AddToRolesAsync(user, roleNames);

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception($"Role ekleme hatası: {errors}");
                    }
                }
                catch (Exception ex)
                {
                    var errorDetails = ex.ToString();
                    return BadRequest($"Bir hata oluştu: {errorDetails}");
                }
            }
            return Ok(new RoleMenu());
        }
        [HttpPost]
        public async Task<IActionResult> RemoveAllUserRoles()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest($"Kullanıcı {user.UserName} için rol(ler) silinemedi: {errors}");
                }
            }

            return Ok(new RoleMenu());
        }
        [HttpPost]
        public async Task<IActionResult> GetMenusWithParentsAndChilds()
        {
            var list = await _menuRepository.GetAll().ToListAsync();
            var infoList = HierarchyService<Menu>.GetHierarchyResults(list);

            var result = infoList.Select(x => new OrganizationDto
            {
                Id = x.EntityId,
                EntryId = x.EntityId,
                Name = x.Name,
                ParentEntityId = x.ParentEntityId,
                Degree = x.Degree,
                Parents = Elements.GetMenus(x.Parents, list),
                SubEntities = Elements.GetMenus(x.SubEntities, list),
            }).Where(x=>x.Degree<2).ToList();

            return Ok(result);
        }

    }
}
