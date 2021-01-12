using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Identity.DbModels
{
    /// <summary>
    /// Join table
    /// </summary>
    public class UserRole
    {
        // Remark: EF Core 5 would enable n:m mappings. This is an option, for to moment we explicitly model join tables as this would allow us to add some audit info etc.

        protected UserRole()
        {

        }

        public UserRole(User user, Role role)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            this.User = user;
            this.Role = role;
            this.Username = user.Username;
            this.RoleId = role.RoleId;
        }


        public int RoleId { get; protected set; }

        public string Username { get; protected set; } = default!;

        public virtual Role Role { get; protected set; } = default!;

        public virtual User User { get; protected set; } = default!;

    }
}
