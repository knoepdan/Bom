using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Bom.Core.Nodes.DbModels
{
    public class Node
    {
        public int NodeId { get; protected set; }

        //   [StringLength(255)]
        public string Title { get; set; } = "";

        public int? MainPathId { get; set; }

        public virtual Path? MainPath { get; set; }

        internal virtual List<Path>? Paths { get; set; }

        public override string ToString()
        {
            var node = $"Node {NodeId} '{Title}'";
#if DEBUG
          if(MainPathId.HasValue && MainPath != null)
            {
                node += $" (ParentPath: '{string.Join(Path.Separator, MainPath.AllNodeIds)}')";
            }
#endif
            return node;
        }
    }
}
