using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Volonterio.Constants;
using Volonterio.Data.Entities;
using Volonterio.Models;

namespace Volonterio.Data.Services
{
    public static class DbSeeder
    {
        public static void SeedAll(this WebApplication app) 
        {
            using (var scope = app.Services.CreateScope()) 
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var context = scope.ServiceProvider.GetRequiredService<EFContext>();

                SeedIdentity(userManager, roleManager, context);
            }
        }

        private static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, EFContext context) 
        {
            if (!roleManager.Roles.Any()) 
            {
                var res = roleManager.CreateAsync(new AppRole { 
                    Name = Roles.USER
                }).Result;

                res = roleManager.CreateAsync(new AppRole { 
                    Name = Roles.ADMIN
                }).Result;
            }

            if (!userManager.Users.Any()) 
            {
                var user = new AppUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    PhoneNumber = "38000000000",
                    FirstName = "Denys",
                    SecondName = "Kravchuk",
                    Image = "1.jpg"
                };
                var res = userManager.CreateAsync(user,"qwerty").Result;

                if(res.Succeeded)
                {
                     var result = userManager.AddToRoleAsync(user, Roles.ADMIN).Result;
                }
            }


        }

        
    }
}
