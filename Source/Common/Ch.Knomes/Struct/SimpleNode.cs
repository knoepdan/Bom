using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ch.Knomes.Struct
{
    public sealed class SimpleNode : ITreeNodeTitle
    {
        private static int idCounter;

        public SimpleNode(string title)
        {
            System.Threading.Interlocked.Increment(ref idCounter);
            this.Id = idCounter;
            this.Title = title;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return $"{Id} '{Title}'";
        }

        public override bool Equals(object? obj)
        {
            var compareNode = obj as SimpleNode;
            if(compareNode != null)
            {
                return (this.Title == compareNode.Title);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if(this.Title != null)
            {
                return this.Title.GetHashCode(StringComparison.InvariantCulture);
            }
            return base.GetHashCode();
        }

        string ITreeNodeTitle.GetTitleString()
        {
            return this.Title;
        }
    }
}