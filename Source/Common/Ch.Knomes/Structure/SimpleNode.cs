using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ch.Knomes.Structure;

namespace Ch.Knomes.Structure
{
    public class SimpleNode : ITreeNodeTitle
    {
        public SimpleNode(string title)
        {
            this.Title = title;
        }

        public string Title { get; set; }

        public override string ToString()
        {
            return "" + this.Title;
        }

        string ITreeNodeTitle.GetTitleString()
        {
            return this.Title;
        }
    }
}