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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable(nameof(Role));
            builder.HasKey(x => x.RoleId);


            builder.Property(t => t.RoleName).HasMaxLength(255);
            builder.Property(x => x.AppFeatures).HasConversion<int>();

            //    builder.HasOne(x => x.MainPath).WithOne().HasForeignKey(typeof(Node), nameof(Node.MainPathId));
        }
    }
}
