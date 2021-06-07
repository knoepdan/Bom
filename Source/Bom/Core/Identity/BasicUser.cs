using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bom.Core.Identity.DbModels;

namespace Bom.Core.Identity
{
    public class BasicUser : IUser
    {
        public BasicUser() { }

        public BasicUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            this.UserId = user.UserId;
            this.Username = user.Username;
            this.Name = user.Name;
        }

        public string Username { get; } = "";

        public string Name { get; } = "";

        public int UserId { get; }

        public override string ToString()
        {
            return this.Username;
        }
    }
}
