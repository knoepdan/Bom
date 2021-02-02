using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Nodes.Models
{
     public class TreeFilterInput
    {
        public int BasePathId { get; set; }

        public int ChildDepth { get; set; }

        public int ParentDepth { get; set; }

    }
}
