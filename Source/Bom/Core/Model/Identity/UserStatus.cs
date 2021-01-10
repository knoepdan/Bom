using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Model.Identity
{
    public enum UserStatus : byte
    {
        MailConfirmationPending = 0,
        Active = 1,

    }
}
