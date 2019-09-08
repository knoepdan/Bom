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
    public class PathConfiguration : IEntityTypeConfiguration<Path>
    {
        public void Configure(EntityTypeBuilder<Path> builder)
        {
            builder.ToTable(nameof(Path));
            builder.HasKey(x => x.PathId);

            builder.Property(x => x.NodePath);
            builder.Property(x => x.Level).ValueGeneratedNever();// calculated column
            builder.Property(x => x.NodePathString).ValueGeneratedNever();// calculated column

            builder.Property(x => x.NodeId);
            builder.AddNodeForeignKey(x => x.Node, nameof(Path.NodeId));


            builder.Ignore(x => x.AllRawNodeIds);
            builder.Ignore(x => x.NofFragments);
        }
    }

    internal static class PathBuilderExtension
    {
        public static void AddNodeForeignKey(this EntityTypeBuilder<Path> builder, Expression<Func<Path, Node>> navigationExpression, string foreignKeyProp)
        {
            builder.HasOne(navigationExpression).WithOne().HasForeignKey(typeof(Path), foreignKeyProp);
        }
    }
}
