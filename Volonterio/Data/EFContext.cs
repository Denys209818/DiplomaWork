using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volonterio.Data.Configuration.Identity;
using Volonterio.Data.Entities;
using Volonterio.Models;

namespace Volonterio.Data
{
    public class EFContext : IdentityDbContext<AppUser, AppRole, long, IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {

        public DbSet<PublicationsModels> publicationsModels { get; set; }
        public DbSet<TagsModels> tagsModels { get; set; }
        public DbSet<PublicationsTagsModel> publicationsTagsModels { get; set; }
        public DbSet<GroupsModels> groupsModels { get; set; }
        public DbSet<UsersModels> usersModels { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public EFContext(DbContextOptions opts) : base(opts)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        {
        
       }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityConfiguration());

            base.OnModelCreating(builder);
            builder.Entity<PublicationsTagsModel>(aspnetroles =>
            {
                aspnetroles.HasKey(asp => new { asp.PublicationsId, asp.TegsId });

                aspnetroles.HasOne(asp => asp.publications).WithMany(a => a.idPublications).HasForeignKey(a => a.PublicationsId).IsRequired();

                aspnetroles.HasOne(asp => asp.tags).WithMany(a => a.idTags).HasForeignKey(a => a.TegsId).IsRequired();
            });

            builder.Entity<PublicationsModels>(aspnetroles =>
            {
                aspnetroles.HasKey(asp => new { asp.Id });
                aspnetroles.HasOne(asp => asp.idgroups).WithMany(a => a.IdgroupsPublications).HasForeignKey(a => a.GroupsId).IsRequired();

            });

            builder.Entity<UsersModels>(aspnetrpoles =>
            {
                aspnetrpoles.HasKey(asp => new { asp.Id });
                aspnetrpoles.HasOne(asp => asp.idgroups).WithMany(a => a.IdGroupsUsers).HasForeignKey(a => a.GroupsId).IsRequired();
            });
        }
    }
}
