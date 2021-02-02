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
    public class DbPictureConfiguration : IEntityTypeConfiguration<DbPicture>
    {
        public void Configure(EntityTypeBuilder<DbPicture> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("nDbPicture");
            builder.HasKey(x => x.DbPictureId);

            builder.Property(x => x.PictureUid);
            builder.Property(x => x.Name);

            builder.Property(x => x.FileLenght);
            builder.Property(x => x.Height);
            builder.Property(x => x.Width);

            builder.Property(x => x.Latitude);
            builder.Property(x => x.Longitude);
            builder.Property(x => x.Place);
            builder.Property(x => x.TimeStamp);

            builder.Property(x => x.BlobDataId);
            builder.HasOne(x => x.BlobData).WithOne().HasForeignKey(typeof(DbPicture), nameof(DbPicture.BlobDataId));
        }
    }
}