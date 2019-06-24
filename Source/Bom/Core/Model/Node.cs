using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class Node
    {
        public int NodeId { get; private set; }

     //   [StringLength(255)]
        public string Title { get; set; }

        public int? MainPathId { get; set; }

        public virtual Path MainPath { get; set; }


        public override string ToString()
        {
            var node = $"Node {NodeId} '{Title}'";
#if DEBUG
          if(MainPathId.HasValue && MainPath != null)
            {
                node += $" (ParentPath: '{string.Join(Path.Separator, MainPath.AllParentNodeIds)}')";
            }
#endif
            return node;
        }
    }
}
