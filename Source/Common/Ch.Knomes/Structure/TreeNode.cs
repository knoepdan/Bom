using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ch.Knomes.Structure
{
    public class TreeNode<T>
    {
        // https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp

        public TreeNode(T data)
        {
            this.Data = data;
        }

        public T Data { get; set; }
        public TreeNode<T> Parent { get; private set; }

        private List<TreeNode<T>> _children = new List<TreeNode<T>>();
        public IReadOnlyList<TreeNode<T>> Children => this._children;

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this._children.Add(childNode);
            return childNode;
        }

        public void MoveToNewParent(TreeNode<T> newParent, bool moveChildrenToo = true)
        {
            if(newParent == null)
            {
                throw new ArgumentNullException(nameof(newParent));
            }

            // check 
            // disallow moving node to its own children
            if(newParent.Ancestors.Any(a => a == this))
            {
                throw new ArgumentException("Cannot move a node down to its own children.. would create an endless loop", nameof(newParent));
            }


            if (!moveChildrenToo)
            {
                // if we don't want to move the children, we only have one option.. we move it to this parent
                foreach(var child in this._children)
                {
                    MoveToNewParent(this.Parent, true);
                }
            }

            // clear
            this.Parent._children.Remove(this);
            this.Parent = null;

            // set new parent // -> we don't use newParent.AddChild(..) because that would create a new instance of TreeNode
            this.Parent = newParent;
            newParent._children.Add(this);
        }

        #region calculated properties

        public int Level
        {
            get
            {
                int level = 1;
                var p = this;
                while (p.Parent != null && p.Parent != this)
                {
                    level++;
                    p = p.Parent;
                }
                return level;
            }
        }

        /// <summary>
        /// All Descendants plus this instance (but no parent nodes)
        /// </summary>
        public IEnumerable<TreeNode<T>> DescendantsAndI
        {
            get
            {
                var list = Descendants;
                list.Add(this);
                return list;
            }
        }

        public List<TreeNode<T>> Descendants
        {
            get
            {
                var list = new List<TreeNode<T>>();
                this.GetAllChildreRecursive(list);
                return list;
            }
        }

        public List<TreeNode<T>> Siblings
        {
            get
            {
                var list = new List<TreeNode<T>>();
                if(this.Parent != null && this.Parent.Children.Count > 1 )
                {
                    list.AddRange(this.Parent.Children.Where(x => x != this));
                }
                return list;
            }
        }

        public List<TreeNode<T>> Ancestors
        {
            get
            {
                var list = new List<TreeNode<T>>();
                var parent = this.Parent;
                while (parent != null && parent != this) // this check to prevent loops
                {
                    list.Add(parent);
                    parent = parent.Parent;
                }
                return list;
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                var root = this;
                var parent = this.Parent;
                while(parent != null && parent != this) // this check to prevent loops
                {
                    root = parent;
                    parent = root.Parent;
                }
                return root;
            }
        }

        public string VisualStringRepresentation 
        {
            get{
                var rootNode = this;
                var sb = new StringBuilder();
                sb.AppendLine(GetVisual(rootNode));
                DrawVisualRecursive(sb, rootNode, 0);
                return sb.ToString();
            }
        }

        private static void DrawVisualRecursive(StringBuilder sb, TreeNode<T> rootNode, int level)
        {
            const string filler = "         ";
            var tmp = new List<String>();
            for (int i = 0; i < level; i++)
            {
                tmp.Add(filler);
            }
            var indentString = string.Join('|', tmp);
            if (!string.IsNullOrEmpty(indentString))
            {
                indentString = "|" + indentString;
            }
            foreach (var n in rootNode.Children)
            {
                var vis = GetVisual(n);
                var line = indentString + "|--- " + vis;
                sb.AppendLine(line);
                DrawVisualRecursive(sb, n, (level + 1));
            }
        }

        private static string GetVisual(TreeNode<T> node)
        {
            var nodeString = node.Data.ToString();
            if(node.Data is ITreeNodeTitle)
            {
                nodeString = ((ITreeNodeTitle)node.Data).GetTitleString();
            }
            return $"[{nodeString}]";
        }


        private void GetAllChildreRecursive(List<TreeNode<T>> list)
        {
            list.AddRange(this.Children);
            foreach (var child in this.Children)
            {
                child.GetAllChildreRecursive(list);
            }
        }
        #endregion


        public override string ToString()
        {
            return $"Node[{Level}] '{ this.Data }'  (Nof children: {Children.Count})";
        }
    }
}