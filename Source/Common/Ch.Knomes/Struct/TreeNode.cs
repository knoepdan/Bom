using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ch.Knomes.Struct
{
    public class TreeNode<T> where T: notnull
    {
        // https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp

        // potential improvments: remove Child (by index or by object), copy method (flag for copying data also or just keep same data)

        public TreeNode(T data, IComparer<TreeNode<T>>? compareFunc = null)
        {
            this.Data = data;
            this.CompareFunc = compareFunc;
        }

        public T Data { get; set; }
        public TreeNode<T>? Parent { get; private set; }

        private List<TreeNode<T>> _children = new List<TreeNode<T>>();
        public IReadOnlyList<TreeNode<T>> Children => this._children;

        /// <summary>
        /// Compare function if nodes should be sorted (Null if unsorted)
        /// </summary>
        public IComparer<TreeNode<T>>? CompareFunc { get; }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child, this.CompareFunc) { Parent = this };
            this._children.Add(childNode);
            if(this.CompareFunc != null)
            {
                this._children.Sort(this.CompareFunc); // probably not very performant this way
            }
            return childNode;
        }

        /// <summary>
        /// Will remove node from the tree completly. (children will be moved up to its parent, or become roots themselves)
        /// </summary>
        public void ExtractNodeFromTree()
        {
            var childList = this._children.ToList();
            if (this.Parent != null)
            {
                foreach (var child in childList)
                {
                    child.MoveToNewParent(this.Parent, true);
                }
                this.Parent.RemoveChild(this);
            }
            else
            {
                // children will all become root nodes
                foreach (var child in childList)
                {
                    child.Parent = null;
                }
                this._children.Clear();
            }
            this.Parent = null;
        }


        public void RemoveChild(TreeNode<T> childToRemove)
        {
            if(childToRemove == null)
            {
                throw new ArgumentNullException(nameof(childToRemove));
            }
            var foundChild = this._children.FirstOrDefault(x => x == childToRemove);
            if(foundChild == null)
            {
                throw new ArgumentException("Passed node is not a child of this node", nameof(childToRemove));
            }
            this._children.Remove(foundChild);
            foundChild.Parent = null;
        }

        public void MoveToNewParent(TreeNode<T> newParent, bool moveChildrenToo = true)
        {
            if (newParent == null)
            {
                throw new ArgumentNullException(nameof(newParent));
            }

            // disallow moving node to its own children
            if (moveChildrenToo && newParent.Ancestors.Any(a => a == this))
            {
                throw new ArgumentException("Cannot move a node down to its own children.. would create an endless loop", nameof(newParent));
            }

            if (!moveChildrenToo)
            {
                // if we don't want to move the children, we only have one option.. we move it to this parent
                var currentChildren = this._children.ToList();
                foreach (var child in currentChildren)
                {
                    if(this.Parent == null)
                    {
                        throw new InvalidOperationException("Parent must be set for children"); // cannot happen
                    }
                    child.MoveToNewParent(this.Parent, true);
                }
            }

            // clear
            if (this.Parent != null)
            {
                this.Parent._children.Remove(this);
            }
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
        public IList<TreeNode<T>> DescendantsAndI
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
                if (this.Parent != null && this.Parent.Children.Count > 1)
                {
                    list.AddRange(this.Parent.Children.Where(x => x != this));
                }
                return list;
            }
        }

        /// <summary>
        /// Ancestors/Parents ordered by proximity to node (e.g. direct parent is first in list)
        /// </summary>
        public IEnumerable<TreeNode<T>> Ancestors
        {
            get
            {
                var parent = this.Parent;
                while (parent != null && parent != this) // this check to prevent loops
                {
                    yield return parent;
                    parent = parent.Parent;
                }
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                var root = this;
                var parent = this.Parent;
                while (parent != null && parent != this) // this check to prevent loops
                {
                    root = parent;
                    parent = root.Parent;
                }
                return root;
            }
        }

        /// <summary>
        /// Compares Data of tree node (beginning with this node and going down to all descendants) on equality using either passed compareMethod or Equals method
        /// </summary>
        /// <remarks>Does not take into account sorting of children</remarks>
        public bool AreDescendantsAndIEqual<TX>(TreeNode<TX> compareNode, Func<TreeNode<T>, TreeNode<TX>, bool>? compareMethod = null) where TX : notnull
        {
            if (compareNode == null)
            {
                throw new ArgumentNullException(nameof(compareNode));
            }

            if (((compareMethod != null && compareMethod(this, compareNode)) ||
                (compareMethod == null && Data != null && Data.Equals(compareNode.Data))) && this.Children.Count == compareNode.Children.Count)
            {
                foreach (var child in this.Children)
                {
                    var alreadyComparedIndex = new List<int>(); // index to keep track if compareChild that already matched a child (otherwise could be true if all children are the same)
                    for (int i = 0; i < compareNode.Children.Count; i++)
                    {
                        if (alreadyComparedIndex.Contains(i))
                        {
                            continue;
                        }
                        var childToCompare = compareNode.Children[i];
                        var childAndDescendantsEqual = child.AreDescendantsAndIEqual(childToCompare, compareMethod);
                        if (childAndDescendantsEqual)
                        {
                            alreadyComparedIndex.Add(i);
                            break;
                        }
                        else if (i == compareNode.Children.Count - 1)
                        {
                            return false; // we compared the last one and compare failed
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a path representation (somethink like: "1/3/32/"
        /// </summary>
        public string GetPath(Func<TreeNode<T>, string> getNodeSegmentFunc, char separator = '/')
        {
            if (getNodeSegmentFunc == null)
            {
                getNodeSegmentFunc = GetNodeDefaultTitle;
            }
            var sb = new StringBuilder();
            var ancestors = this.Ancestors.ToList();
            for (int i = ancestors.Count - 1; i >= 0; i--)
            {
                var anc = ancestors[i];
                sb.Append(getNodeSegmentFunc(anc));
                sb.Append(separator);
            }
            sb.Append(getNodeSegmentFunc(this));
            return sb.ToString();
        }

        public string VisualString => VisualStringRepresentation(null);

        public string VisualStringRepresentation(Func<TreeNode<T>, string>? getNodeTitleFunc = null)
        {
            if (getNodeTitleFunc == null)
            {
                getNodeTitleFunc = GetNodeDefaultTitle;
            }

            var rootNode = this;
            var sb = new StringBuilder();
            sb.AppendLine(getNodeTitleFunc(rootNode));
            DrawVisualRecursive(sb, rootNode, getNodeTitleFunc, true);
            return sb.ToString();
        }

        private static void DrawVisualRecursive(StringBuilder sb, TreeNode<T> rootNode, Func<TreeNode<T>, string> getNodeTitleFunc, bool treatAsRootNode = false)
        {
            string indentString = "";
            const string filler = "         ";

            // collect info if "|" has to be drawn in indent string
            var nodeToCheck = rootNode;
            var hasChildList = new List<bool>();
            while (nodeToCheck.Parent != null && !treatAsRootNode)
            {
                var newNodeToCheck = nodeToCheck.Parent;
                var orderedChildren = newNodeToCheck._children;
                var index = orderedChildren.IndexOf(nodeToCheck);
                hasChildList.Add(index < newNodeToCheck._children.Count - 1);
                nodeToCheck = newNodeToCheck;
            }

            // draw indent string
            hasChildList.Reverse();
            foreach (var mark in hasChildList)
            {
                if (mark)
                {
                    indentString = indentString + "|" + filler;
                }
                else
                {
                    indentString = indentString + " " + filler;
                }
            }

            // draw lines
            var rootNodeOrderedChildren = rootNode.Children;
            foreach (var n in rootNodeOrderedChildren)
            {
                var nodeTitle = getNodeTitleFunc(n);
                var line = indentString + "|--- " + nodeTitle;
                sb.AppendLine(line);
                DrawVisualRecursive(sb, n, getNodeTitleFunc);
            }
        }

        private static string GetNodeDefaultTitle(TreeNode<T> node)
        {
            var nodeString = node.Data?.ToString();
            if (node.Data is ITreeNodeTitle)
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