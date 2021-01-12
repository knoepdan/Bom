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
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable(nameof(UserRole));
            builder.HasKey(x => new { x.RoleId, x.Username });
            builder.Property(x => x.RoleId);
            builder.Property(x => x.Username);


            builder.HasOne(p => p.Role).WithMany().HasForeignKey(x => x.RoleId);
            builder.HasOne(p => p.User).WithMany(us => us.UserRoles).HasForeignKey(x => x.Username);
            
        }
    }
}
