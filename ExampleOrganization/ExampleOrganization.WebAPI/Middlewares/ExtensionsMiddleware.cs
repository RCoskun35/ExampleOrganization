﻿using ExampleOrganization.Domain.Entities;
using ExampleOrganization.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace ExampleOrganization.WebAPI.Middlewares
{
    public static class ExtensionsMiddleware
    {
        public static void Seeds(WebApplication app)
        {
            using (var scoped = app.Services.CreateScope())
            {
                var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!userManager.Users.Any(p => p.UserName == "admin"))
                {
                    
                    AppUser user = new()
                    {
                        UserName = "admin",
                        Email = "admin@admin.com",
                        FirstName = "Ridvan",
                        LastName = "Coskun",
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(user, "1").Wait();

                    for (int i = 1; i < 30; i++)
                    {
                        AppUser user2 = new()
                        {
                            UserName = "user"+i.ToString(),
                            Email = $"user{i}@user.com",
                            FirstName = $"user{i}",
                            LastName = "user",
                            EmailConfirmed = true
                        };

                        userManager.CreateAsync(user2, "1").Wait();
                    }
                }
                if(!context.Menus.Any())
                {
                   
                }
                

            }
        }
    }
}
