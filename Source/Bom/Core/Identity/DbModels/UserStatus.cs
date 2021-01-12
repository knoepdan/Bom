using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Identity.DbModels
{
    public enum UserStatus : byte
    {
        MailConfirmationPending = 0,
        Active = 1,

    }
}
