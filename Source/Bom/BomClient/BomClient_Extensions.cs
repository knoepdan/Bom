using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BomClient
{
    public partial class NodeVm
    {
        public int Depth
        {
            get
            {
                if(this.Path == null)
                {
                    return 0;
                }
                return this.Path.Count(x => x == '/');
            }
        }
        public override string ToString()
        {
            return $"{this.Path} - '{this.Title}'";
        }
    }
}
