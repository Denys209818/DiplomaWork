using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Volonterio.Constants;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
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
                var res = userManager.CreateAsync(new AppUser { 
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    PhoneNumber = "38000000000",
                    FirstName = "Denys",
                    SecondName = "Kravchuk",
                    Image = "1.jpg"
                }).Result;
            }


                    },
                    new UsersModels
                    {
                        Email = "TestEmail_1@gmail.com",
                        FirstName = "Дмитро",
                        SecondName = "Чайник",
                        Image = "4ainik.jpg",
                        Phone = "098-003-45-89",
                        Password = "password1-",
                        GroupsId = 2
                    }
                };
                context.usersModels.AddRange(users);
                context.SaveChanges();
            }
                    },
                    new UsersModels
                    {
                        Email = "TestEmail_1@gmail.com",
                        FirstName = "Дмитро",
                        SecondName = "Чайник",
                        Image = "4ainik.jpg",
                        Phone = "098-003-45-89",
                        Password = "password1-",
                        GroupsId = 2
                    }
                };
                context.usersModels.AddRange(users);
                context.SaveChanges();
            }
                    },
                    new UsersModels
                    {
                        Email = "TestEmail_1@gmail.com",
                        FirstName = "Дмитро",
                        SecondName = "Чайник",
                        Image = "4ainik.jpg",
                        Phone = "098-003-45-89",
                        Password = "password1-",
                        GroupsId = 2
                    }
                };
                context.usersModels.AddRange(users);
                context.SaveChanges();
            }
        }

        
    }
}
