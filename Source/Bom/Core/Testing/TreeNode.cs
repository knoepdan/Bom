using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.Testing
{
    public class TreeNode<T>
    {
        // https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp

        public T Data { get; set; }
        public TreeNode<T> Parent { get; private set; }

        private List<TreeNode<T>> _children = new List<TreeNode<T>>();
        public IReadOnlyList<TreeNode<T>> Children => this._children;

        public int Level
        {
            get
            {
                int level = 1;
                var p = this;
                while (p.Parent != null)
                {
                    level++;
                    p = p.Parent;
                }
                return level;
            }

        }

        public TreeNode(T data)
        {
            this.Data = data;
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this._children.Add(childNode);
            return childNode;
        }

        #region iterating

        public IEnumerable<TreeNode<T>> AllNodes
        {
            get
            {
                var list = new List<TreeNode<T>>();
                list.Add(this);
                this.GetAllChildreRecursive(list);
                return list;
            }
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