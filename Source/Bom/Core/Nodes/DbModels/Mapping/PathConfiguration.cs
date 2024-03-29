﻿using System;
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
    public class PathConfiguration : IEntityTypeConfiguration<Path>
    {
        public void Configure(EntityTypeBuilder<Path> builder)
        {
            if(builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("nPath");
            builder.HasKey(x => x.PathId);

            builder.Property(x => x.Level).ValueGeneratedNever();// calculated column
            builder.Property(x => x.NodePathString).ValueGeneratedNever();// calculated column

            builder.Property(x => x.NodeId);
            builder.HasOne(p => p.Node).WithMany(n => n.Paths); // // builder.AddNodeForeignKey(x => x.Node, nameof(Path.NodeId));


            builder.Ignore(x => x.AllRawNodeIds);
            builder.Ignore(x => x.NofFragments);
            //builder.Property(x => x.NodePath); -> outcommented because not working in .net core 3.0
        }
    }

    internal static class PathBuilderExtension
    {
        public static void AddNodeForeignKey(this EntityTypeBuilder<Path> builder, Expression<Func<Path, Node?>> navigationExpression, string foreignKeyProp)
        {
            builder.HasOne(navigationExpression).WithOne().HasForeignKey(typeof(Path), foreignKeyProp);
        }
    }
}
