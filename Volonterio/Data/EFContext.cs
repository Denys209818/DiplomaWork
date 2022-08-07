using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volonterio.Data.Configuration;
using Volonterio.Data.Configuration.Identity;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
using Volonterio.Models;

namespace Volonterio.Data
{
    public class EFContext : IdentityDbContext<AppUser, AppRole, long, IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {


        /// <summary>
        /// App entities
        /// </summary>
        public DbSet<AppGroup> Groups { get; set; }
        public DbSet<AppTag> Tags { get; set; }
        public DbSet<AppPostImage> PostImages { get; set; }
        public DbSet<AppPostTag> PostTags { get; set; }
        public DbSet<AppPost> Post { get; set; }
        public DbSet<AppUserFriend> UserFriends { get; set; }
        public DbSet<AppFriend> Friends { get; set; }
        public DbSet<AppPostTagEntity> PostTagEntities { get; set; }
        public DbSet<AppGroupTag> GroupTags { get; set; }

        public DbSet<MessageModels> messages { get; set; }
        public DbSet<FriendChatModels> friendChats { get; set; }
        public DbSet<GroupChatModels> groupChats { get; set; }

        public DbSet<MessageModels> messages { get; set; }
        public DbSet<FriendChatModels> friendChats { get; set; }
        public DbSet<GroupChatModels> groupChats { get; set; }

        public EFContext(DbContextOptions opts) : base(opts)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityConfiguration());
            builder.ApplyConfiguration(new GroupConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new PostImageConfiguration());
            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new PostTagConfiguration());
            builder.ApplyConfiguration(new UserFriendConfiguration());
            builder.ApplyConfiguration(new FriendConfiguration());
            builder.ApplyConfiguration(new PostTagEntitiesConfiguration());
            builder.ApplyConfiguration(new GroupTagConfiguration());




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
