using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class FriendChatConfiguration : IEntityTypeConfiguration<AppFriendChat>
    {
        public void Configure(EntityTypeBuilder<AppFriendChat> builder)
        {
            builder.ToTable("tblAppFriendChat");

            builder.HasKey(x => x.Id);
        }
    }
}
