using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bom.Core.Model.Identity;

namespace Bom.Core.Data.Mapping.Identity
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
            builder.HasKey(x => x.Username);


            builder.Property(t => t.Username).HasMaxLength(255);

            builder.Property(x => x.PasswordHash);
            builder.Property(x => x.Salt);
            builder.Property(x => x.ActivationToken);
            builder.Property(x => x.FacebookId);
            builder.Property(x => x.UserStatus).HasConversion<byte>();
        }
    }
}
