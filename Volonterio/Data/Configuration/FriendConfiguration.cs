using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class FriendConfiguration : IEntityTypeConfiguration<AppFriend>
    {
        public void Configure(EntityTypeBuilder<AppFriend> builder)
        {
            builder.ToTable("tblAppFriend");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.UserFriend)
                .WithMany(x => x.AppFriends)
                .HasForeignKey(x => x.UserFriendId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
