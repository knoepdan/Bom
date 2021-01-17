using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bom.Core.Identity.DbModels;

namespace Bom.Core.Identity.DbModels.Mapping
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable(nameof(User));
            builder.HasKey(x => x.UserId);

            builder.Property(t => t.UserId);

            builder.Property(t => t.Username).HasMaxLength(255);

            builder.Property(x => x.PasswordHash);
            builder.Property(x => x.Salt);
            builder.Property(x => x.ActivationToken);
            builder.Property(x => x.FacebookId);
            builder.Property(x => x.UserStatus).HasConversion<byte>();

            builder.Property(t => t.Email2).HasMaxLength(255);
            builder.Property(t => t.Name).HasMaxLength(255);

        }
    }
}
