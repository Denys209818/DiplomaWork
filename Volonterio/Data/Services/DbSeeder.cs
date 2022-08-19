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

            if(!context.groupsModels.Any())
            {
                var group = new List<GroupsModels>()
                {
                    new GroupsModels
                    {
                        Name = "Group Test",
                        Description = "Group Test",
                        Target = "Group Test"
                    },
                    new GroupsModels
                    {
                        Name = "Group Test_1",
                        Description = "Group Test_1",
                        Target = "Group Test_1"
                    },
                    new GroupsModels
                    {
                        Name = "Group Test_2",
                        Description = "Group Test_2",
                        Target = "Group Test_2"
                    }
                };
                context.groupsModels.AddRange(group);
                context.SaveChanges();
            }

            if(!context.publicationsModels.Any())
            {
                var publications = new List<PublicationsModels>()
                {
                    new PublicationsModels
                    {
                        Name = "PblTest_1",
                        TegsSearch = "#test1",
                        GroupsId = 2,
                        Image = "TestImage1.jpg"
                    },
                    new PublicationsModels
                    {
                        Name = "PblTest_2",
                        TegsSearch = "#test2",
                        GroupsId = 1,
                        Image = "TestImage2.jpg"
                    },
                    new PublicationsModels
                    {
                        Name = "PblTest_3",
                        TegsSearch = "#test3",
                        GroupsId = 3,
                        Image = "TestImage3.jpg"
                    }
                };
                context.publicationsModels.AddRange(publications);
                context.SaveChanges();
            }

            if(!context.tagsModels.Any())
            {
                var tags = new List<TagsModels>()
                {
                    new TagsModels
                    {
                        NameTags = "#test1"
                    },
                    new TagsModels
                    {
                        NameTags = "#test2"
                    },
                    new TagsModels
                    {
                        NameTags = "#test3"
                    }
                };
                context.tagsModels.AddRange(tags);
                context.SaveChanges();
            }

            if (!context.publicationsTagsModels.Any())
            {
                var publicationstags = new List<PublicationsTagsModel>()
                {
                    new PublicationsTagsModel
                    {
                        PublicationsId = 1,
                        TegsId = 2
                    },
                    new PublicationsTagsModel
                    {
                        PublicationsId = 2,
                        TegsId = 1
                    }
                };
                context.publicationsTagsModels.AddRange(publicationstags);
                context.SaveChanges();
            }
            if(!context.messages.Any())
            {
                MessageModels messageModels = new MessageModels
                {
                    Message = "Hello",
                    UserId = 1,
                    ChadId = 2,
                    DateCreated = DateTime.UtcNow,
                    DateCreatedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                context.messages.Add(messageModels);
                context.SaveChanges();
            }
            
        }

        
    }
}
