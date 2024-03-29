﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bom.Core.Nodes.DbModels;

namespace Bom.Core.Nodes.DbModels.Mapping
{
    public class NodeConfiguration : IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            if(builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("nNode");
            builder.HasKey(x => x.NodeId);


            builder.Property(t => t.Title).HasMaxLength(255);
            builder.Property(x => x.MainPathId);

            builder.HasOne(x => x.MainPath).WithOne().HasForeignKey(typeof(Node), nameof(Node.MainPathId));
        }
    }
}
