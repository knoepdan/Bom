using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bom.Core.Model.Identity
{
    public class Role
    {
        public int RoleId { get; protected set; }

        public string RoleName { get; set; } = "";

        public AppFeature AppFeatures { get; set; }

        public bool HasFeature(params AppFeature[] features)
        {
            foreach (var feature in features)
            {
                if (AppFeatures.HasFlag(feature))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
