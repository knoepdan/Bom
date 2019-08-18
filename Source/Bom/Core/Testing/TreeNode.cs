using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.Testing
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        // https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp

        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public ICollection<TreeNode<T>> Children { get; set; }

        public int Level
        {
            get
            {
                int level = 1;
                var p = this;
                while(p.Parent != null)
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
            this.Children = new LinkedList<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this.Children.Add(childNode);
            return childNode;
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        #region iterating

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        public IEnumerator<TreeNode<T>> GetGenericEnumerator()
        {
            yield return this;
            foreach (var directChild in this.Children)
            {
                foreach (var anyChild in directChild)
                {
                    yield return anyChild;
                }
            }
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator()
        {
           return this.GetGenericEnumerator();
        }
    }
}