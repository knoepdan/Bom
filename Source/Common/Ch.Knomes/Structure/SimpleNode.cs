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

        public override bool Equals(object obj)
        {
            var compareNode = obj as SimpleNode;
            if(compareNode != null )
            {
                return (this.Title == compareNode.Title);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if(this.Title != null)
            {
                return this.Title.GetHashCode();
            }
            return base.GetHashCode();
        }

        string ITreeNodeTitle.GetTitleString()
        {
            return this.Title;
        }
    }
}