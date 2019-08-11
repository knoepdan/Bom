using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Testing
{

    public class MemoryNode
    {
        public MemoryNode(string title)
        {
            this.Title = title;
        }

        public string Title { get; set; }

        public override string ToString()
        {
            return "" + this.Title;
        } 
    }

    
}
