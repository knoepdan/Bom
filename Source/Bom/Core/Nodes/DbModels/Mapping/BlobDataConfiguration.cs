using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bom.Core.Nodes.DbModels;

namespace Bom.Core.Nodes.DbModels.Mapping
{
    public class BlobDataConfiguration : IEntityTypeConfiguration<BlobData>
    {
        public void Configure(EntityTypeBuilder<BlobData> builder)
        {
            if(builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("nBlobData");
            builder.HasKey(x => x.BlobDataId);
            builder.Property(x => x.Data);
        }
    }
}