using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Domain.Repositories;
using ExampleOrganization.Infrastructure.Modules;
using ExampleOrganization.WebAPI.Abstractions;
using ExampleOrganization.WebAPI.AuthorizationFilter;
using GenericHierarchy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExampleOrganization.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModulesController : ApiController
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRoleModuleRepository _roleModuleRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private  readonly IMemoryCache _memoryCache;
        public ModulesController(IMediator mediator, RoleManager<Role> roleManager, IRoleModuleRepository roleModuleRepository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IMemoryCache memoryCache) : base(mediator)
        {
            _roleManager = roleManager;
            _roleModuleRepository = roleModuleRepository;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
        }





        [HttpPost]
        public IActionResult GetModules()
        {
            return Ok(ModuleRepository.GetModules());
        }
        [HttpPost]
        public IActionResult GetModulesWithChildrenAndParents()
        {
            var modules = HierarchyService<Module>.GetHierarchyResults(ModuleRepository.GetModules());

            var result = modules.Select(x => new
            {
                x.Id,
                x.Name,
                ParentName = ModuleRepository.ModuleName(x.ParentEntityId ?? 0),
                x.Degree,
                x.EntityId,
                SubEntities = x.SubEntities.Select(a => ModuleRepository.ModuleName(a)).ToList(),
                Parents = x.Parents.Select(a => ModuleRepository.ModuleName(a)).ToList(),
            });

            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddModule(Module module)
        {
            return Ok(ModuleRepository.AddModule(module));
        }
        [HttpPost("{moduleId}")]
        public IActionResult RemoveModule(int moduleId)
        {
            return Ok(ModuleRepository.RemoveModule(moduleId));
        }
        [HttpPost]
        public IActionResult UpdateModule(Module module)
        {
            return Ok(ModuleRepository.UpdateModule(module));
        }


        [HttpPost]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleModules = await _roleModuleRepository.GetAll().ToListAsync();
            var modules = ModuleRepository.GetModules();
            var result = roles.Select(role => new
            {
                role.Id,
                role.Name,
                Modules = roleModules
                    .Where(rm => rm.RoleId == role.Id)
                    .Select(rm => modules.FirstOrDefault(m => m.Id == rm.ModuleId)?.Name)
                    .ToList()
            }).ToList();

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleModules(RoleAndModules roleAndModules)
        {
            var roleModules = await _roleModuleRepository.Where(x => x.RoleId == roleAndModules.RoleId).ToListAsync();
            if (roleModules.Any()) {
                _roleModuleRepository.DeleteRange(roleModules);
                await _roleModuleRepository.SaveChangesAsync();

            }
            var roleModuleList = new List<RoleModule>();
            foreach (var moduleId in roleAndModules.ModuleIds)
            {
                roleModuleList.Add(new RoleModule { ModuleId = moduleId, RoleId = roleAndModules.RoleId });
            }
            await _roleModuleRepository.AddRangeAsync(roleModuleList);
            await _roleModuleRepository.SaveChangesAsync();
            return Ok(new List<RoleModule>());
        }

        [HttpPost]
        public async Task<IActionResult> GetUsers()
        {

            var userModulesList = new List<UserAndModules>();

            var users = await _userManager.Users.ToListAsync();
            var rolList = await _roleManager.Roles.ToListAsync();
            var hierarchyModules = HierarchyService<Module>.GetHierarchyResults(ModuleRepository.GetModules());

            try
            {
                foreach (var user in users)
                {
                    var userModule = new UserAndModules();
                    userModule.User = user;
                    var roles = (await _userManager.GetRolesAsync(user)).ToList();
                    foreach (var role in roles)
                    {
                        userModule.Roles.Add(role);
                        var roleId = rolList.Where(r => r.Name == role).Select(a => a.Id).FirstOrDefault();
                        var modules = await _roleModuleRepository.Where(x => x.RoleId == roleId).Select(x => x.ModuleId).ToListAsync();
                        if (modules.Count > 0)
                        {
                            var list = hierarchyModules.Where(x => modules.Contains(x.EntityId)).SelectMany(a => a.SubEntities.Select(y => ModuleRepository.ModuleName(y))).ToList();
                            userModule.Modules = list;
                        }

                    }
                    userModulesList.Add(userModule);

                }

            }
            catch (Exception ex)
            {
                var asd = ex.Message;
                throw;
            }
            return Ok(userModulesList);
        }
        [HttpPost]
        public async Task<IActionResult> GetUser()
        {
            var userId = Convert.ToInt32(_contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value.ToString());
            var user = await _userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var userModule = new UserAndModules();
            var rolList = await _roleManager.Roles.ToListAsync();
            var hierarchyModules = HierarchyService<Module>.GetHierarchyResults(ModuleRepository.GetModules());

            try
            {
                if (user != null) {
                    var appUser = new AppUser();
                    appUser.Id = userId;
                    appUser.FirstName = user.FirstName;
                    appUser.LastName = user.LastName;
                    userModule.User = appUser;
                    var roles = (await _userManager.GetRolesAsync(user)).ToList();
                    foreach (var role in roles)
                    {
                        userModule.Roles.Add(role);
                        var roleId = rolList.Where(r => r.Name == role).Select(a => a.Id).FirstOrDefault();
                        var modules = await _roleModuleRepository.Where(x => x.RoleId == roleId).Select(x => x.ModuleId).ToListAsync();
                        if (modules.Count > 0)
                        {
                            var list = hierarchyModules.Where(x => modules.Contains(x.EntityId)).SelectMany(a => a.SubEntities.Select(y => y.ToString())).ToList();
                            userModule.Modules = list;

                            var list2 = hierarchyModules.Where(x => modules.Contains(x.EntityId)).SelectMany(a => a.Parents.Select(y => y.ToString())).ToList();
                            userModule.Parents = list2;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var asd = ex.Message;
                throw;
            }
            _memoryCache.Set(user!.Id.ToString(), userModule);
            return Ok(userModule);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserRoles(UserAndRoles userAndRoles)
        {
            var user = await _userManager.Users.Where(x => x.Id == userAndRoles.UserId).FirstOrDefaultAsync();
            var userRoles = await _userManager.GetRolesAsync(user!);

            if (userRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user!, userRoles);
            }
            var addRoles = userAndRoles.RoleIds.Select(x => _roleManager.Roles.Where(r => r.Id == x).Select(a => a.Name).FirstOrDefault()).ToList();
            if (addRoles.Any())
            {
                await _userManager.AddToRolesAsync(user!, addRoles!);
            }
            return Ok(new List<RoleModule>());
        }

        [HttpPost]
        [ModuleFilter(ModuleType.AylikRaporEkle)]
        public IActionResult AddReport()
        {
          
            return Ok();
        }
        [HttpPost]
        [ModuleFilter(ModuleType.AylikRaporGuncelle)]
        public IActionResult EditReport()
        {

            return Ok();
        }
    }

    
}
