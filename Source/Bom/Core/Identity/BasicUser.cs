using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bom.Core.Identity.DbModels;

namespace Bom.Core.Identity
{
    public class BasicUser
    {
        public BasicUser() { }

        public BasicUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            this.UserId = user.UserId;
            this.UserName = user.Username;
            this.Name = user.Name;
        }

        public string UserName { get; } = "";

        public string Name { get; } = "";

        public int UserId { get; }

        public override string ToString()
        {
            return this.UserName;
        }
    }
}
