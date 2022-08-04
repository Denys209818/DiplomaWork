using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class UserFriendConfiguration : IEntityTypeConfiguration<AppUserFriend>
    {
        public void Configure(EntityTypeBuilder<AppUserFriend> builder)
        {
            builder.ToTable("tblAppUserFriend");

            builder.HasKey(x => x.Id);
        }
    }
}
