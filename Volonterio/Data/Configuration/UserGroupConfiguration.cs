using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<AppUserGroup>
    {
        public void Configure(EntityTypeBuilder<AppUserGroup> builder)
        {
            builder.ToTable("tblAppUserGroup");
            builder.HasKey(x => new {x.GroupId, x.UserId});

            builder.HasOne(x => x.User)
               .WithMany(x => x.UserGroups).HasForeignKey(x => x.UserId)
               .IsRequired();

            builder.HasOne(x => x.Group)
               .WithMany(x => x.UserGroups).HasForeignKey(x => x.GroupId)
               .IsRequired();
        }
    }
}
