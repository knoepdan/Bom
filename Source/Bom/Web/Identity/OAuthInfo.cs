using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Identity
{
    public class OAuthInfo
    {

        public enum OAuthType : byte
        {
            Undefined = 0,
            Facebook = 1,
            // Google, Microsoft etc.

        }

        public OAuthInfo(OAuthType type, string identifier)
        {
            this.Type = type;
            this.Identifier = identifier;

        }

        public OAuthType Type { get; set; } = OAuthType.Undefined;

        public string? Identifier { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }


    }
}
