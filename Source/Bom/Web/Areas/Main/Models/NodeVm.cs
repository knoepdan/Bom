using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Main.Models
{
    public class NodeVm
    {
        public NodeVm() { }

        public NodeVm(Core.Model.Path path)
        {
            if(path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // map
            this.NodeId = path.NodeId;
            this.Title = path.Node.Title;
            this.Path = path.NodePathString;
            this.PathId = path.PathId;
            this.MainPathId = path.Node.MainPathId;

        }

        public int NodeId { get; set; }

        public string Title { get; set; }

        public int? MainPathId { get; set; }

        public string Path { get; set; }
        
        public int PathId { get; set; }

    }
}
