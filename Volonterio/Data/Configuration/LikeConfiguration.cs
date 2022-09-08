using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class LikeConfiguration : IEntityTypeConfiguration<AppLike>
    {
        public void Configure(EntityTypeBuilder<AppLike> builder)
        {
            builder.ToTable("tblAppLikes");

            builder.HasKey(x => new {x.UserId, x.PostId});

            builder.HasOne(x => x.User)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Post)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.PostId)
                .IsRequired();
        }
    }
}
