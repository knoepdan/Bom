using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Identity.DbModels
{
    public class User : IUser
    {

        public int UserId { get; set; }

        /// <summary>
        /// Username/Email
        /// </summary>
        public string Username { get; set; } = "";

        public string? PasswordHash { get; set; }

        public string? Salt { get; set; }

        public string? ActivationToken { get; set; }

        public string? FacebookId { get; set; }

        public UserStatus UserStatus { get; set; } = UserStatus.Initializing;

        public string? Email2 { get; set; }

        public string Name { get; set; } = "";

        internal virtual IList<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public bool HasFeature(params AppFeature[] features)
        {
            if(UserRoles == null)
            {
                return false;
            }
            foreach (var userRole in UserRoles)
            {
                if(userRole.Role == null)
                {
                    continue;
                }
                if (userRole.Role.HasFeature(features))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddRole(Role role)
        {
            if(role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var userRole = new UserRole(this, role);
            this.UserRoles.Add(userRole);
        }

    }
}
