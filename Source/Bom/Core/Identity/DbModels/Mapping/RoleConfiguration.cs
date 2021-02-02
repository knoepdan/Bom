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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("iRole");
            builder.HasKey(x => x.RoleId);


            builder.Property(t => t.RoleName).HasMaxLength(255);
            builder.Property(x => x.AppFeatures).HasConversion<int>();

            //    builder.HasOne(x => x.MainPath).WithOne().HasForeignKey(typeof(Node), nameof(Node.MainPathId));
        }
    }
}
