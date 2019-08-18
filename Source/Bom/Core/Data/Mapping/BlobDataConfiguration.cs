using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bom.Core.Model;

namespace Bom.Core.Data.ModelMapping
{
    public class BlobDataConfiguration : IEntityTypeConfiguration<BlobData>
    {
        public void Configure(EntityTypeBuilder<BlobData> builder)
        {
            builder.ToTable(nameof(BlobData));
            builder.HasKey(x => x.BlobDataId);
            builder.Property(x => x.Data);
        }
    }
}