using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bom.Core.Configuration.DbModels.Mapping
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("cSetting");
            builder.HasKey(x => x.SettingId);
            builder.Property(x => x.Key);
            builder.Property(x => x.Value);
        }
    }
}