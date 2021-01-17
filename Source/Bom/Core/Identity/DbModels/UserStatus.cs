using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Identity.DbModels
{
    public enum UserStatus : byte
    {
        Initializing = 0,
        MailConfirmationPending = 1,
        Active = 2,

    }
}
