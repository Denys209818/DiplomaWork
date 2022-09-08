using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class FriendMessageConfiguration : IEntityTypeConfiguration<AppFriendMessage>
    {
        public void Configure(EntityTypeBuilder<AppFriendMessage> builder)
        {
            builder.ToTable("tblAppFriendMessage");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(x => x.UserFriend)
                .WithMany(x => x.FriendMessages)
                .HasForeignKey(x => x.UserFriendId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.FriendMessages)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
