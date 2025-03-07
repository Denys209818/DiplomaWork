﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Configuration
{
    public class PostTagConfiguration : IEntityTypeConfiguration<AppPostTag>
    {
        public void Configure(EntityTypeBuilder<AppPostTag> builder)
        {
            builder.ToTable("tblAppPostTag");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Tag)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
