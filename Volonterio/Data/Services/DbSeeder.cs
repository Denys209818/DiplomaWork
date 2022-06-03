using Microsoft.AspNetCore.Identity;
using Volonterio.Constants;
using Volonterio.Data.Entities;

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

                SeedIdentity(userManager, roleManager);
            }
        }

        private static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) 
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
                var res = userManager.CreateAsync(new AppUser { 
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    PhoneNumber = "38000000000",
                    FirstName = "Denys",
                    SecondName = "Kravchuk",
                    Image = "1.jpg"
                }).Result;
            }
        }

        
    }
}
