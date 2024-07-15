using ExampleOrganization.Domain.Dtos;
using ExampleOrganization.Infrastructure.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace ExampleOrganization.WebAPI.AuthorizationFilter
{
   
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModuleFilterAttribute : TypeFilterAttribute
    {
        public ModuleFilterAttribute(ModuleType moduleType) : base(typeof(ModuleFilterImpl))
        {
            Arguments = new object[] { moduleType };
        }

        private class ModuleFilterImpl : IActionFilter
        {
            private readonly ModuleType _moduleType;
            private readonly IMemoryCache _memoryCache;

            public ModuleFilterImpl(ModuleType moduleType, IMemoryCache memoryCache)
            {
                _moduleType = moduleType;
                _memoryCache = memoryCache;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                var accessModule = (int)_moduleType;
                if (userId != null)
                {
                    var userModules = _memoryCache.Get<UserAndModules>(userId);
                    if (!userModules!.Modules.Contains(accessModule.ToString())){
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    Console.WriteLine($"Executing action for module: {_moduleType}");
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                Console.WriteLine($"Executed action for module: {_moduleType}");
            }
        }
    }
}
