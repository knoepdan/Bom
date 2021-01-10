using System;
using System.Linq;
using Xunit;
using Bom.Core.TestUtils;
using Bom.Core.Data;
using Bom.Core.Model;
using Bom.Core.Model.Identity;
using Ch.Knomes.Struct;
using Bom.Core.TestUtils.Models;

namespace Bom.Core.Data.Identity
{
    public class IdentityTests
    {

        [Fact]
        public void Identity_mappings_work()
        {
            using (var context = TestHelpers.GetModelContext(true))
            {

                const string userName = "testUser23498duofs";
                const string roleName = "testRole234kj234mn0";

                // ensure test elements don't exist
                var relevantUser = context.Users.Where(x => x.Username == userName);
                var relevantRole = context.Roles.Where(x => x.RoleName == roleName);
                context.RemoveRange(relevantUser);
                context.RemoveRange(relevantRole);
                context.SaveChanges();

                // elements in db
                var user = new User();
                user.Username = userName;
                user.Salt = "bla";
                user.PasswordHash = "xx";
                context.Users.Add(user);

                var role = new Role();
                role.RoleName = roleName;
                role.AppFeatures = AppFeature.EditOther;
                context.Roles.Add(role);
                context.SaveChanges();
                user.AddRole(role);
                context.SaveChanges();

                // test if 
                var savedUser = context.Users.FirstOrDefault(x => x.Username == userName);
                Assert.True(savedUser != null && savedUser.Username == userName && savedUser.HasFeature(AppFeature.EditOther));
                var savedRole = context.Roles.FirstOrDefault(x => x.RoleName == roleName);
                Assert.True(savedRole != null && savedRole.RoleName == roleName);
                var savedUserRoles = context.UserRoles.Where(x => x.Username == userName);
                Assert.True(savedUserRoles.Count() == 1 && savedUserRoles.First().RoleId == savedRole?.RoleId);

                // clear up
                context.RemoveRange(relevantUser);
                context.RemoveRange(relevantRole);
                context.SaveChanges();
            }
        }
    }
}