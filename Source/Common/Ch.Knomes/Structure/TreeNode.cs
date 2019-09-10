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

            // disallow moving node to its own children
            if(moveChildrenToo && newParent.Ancestors.Any(a => a == this))
            {
                throw new ArgumentException("Cannot move a node down to its own children.. would create an endless loop", nameof(newParent));
            }


            if (!moveChildrenToo)
            {
                // if we don't want to move the children, we only have one option.. we move it to this parent
                var currentChildren = this._children.ToList();
                foreach(var child in currentChildren)
                {
                    child.MoveToNewParent(this.Parent, true);
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
                DrawVisualRecursive(sb, rootNode);
                return sb.ToString();
            }
        }

        private static void DrawVisualRecursive(StringBuilder sb, TreeNode<T> rootNode)
        {
            string indentString = "";
            const string filler = "         ";

            // simple but incorrect version of indent string (writing too many |)
            //var tmp = new List<String>();
            //for (int i = 0; i < rootNode.Level-1; i++)
            //{
            //    tmp.Add(filler);
            //}
            //indentString = string.Join('|', tmp);
            //if (!string.IsNullOrEmpty(indentString))
            //{
            //    indentString = "|" + indentString;
            //}


            // variant 2 TODO buggy.. not working
            // probably solution  -> go up one level and see if after this node there are still children. [check1].. recursive step.. parent.parent and see if after parent are still childre [check2] etc. etc.
            /*
             
                    [Animals]
                    |--- [Mammals]
                    |         |--- [Xenarthra]
                                        |--- [Pilosa]
                                                  |--- [Anteater]
                                                  |--- [Three-toed sloths]
                                                  |--- [Twoe-toed sloths]
                    |--- [Birds]
                    |         |--- [Songbirds]
                    |         |--- [Non-Songbirds]
                    |--- [Reptiles]
                              |--- [Turtles]
                              |--- [Lizards]
                              |--- [Snakes]
                              |--- [Crocodilians]    (level 3)
                                        |--- [Alligators]
                                        |--- [Crocodiles]
                                        |--- [Gavial]
                              |--- [Amphibians]

            */
            var markerArry = new bool[rootNode.Level-1];
            if(rootNode.Parent != null)
            {
                bool foundMe = false;
                int counter = -1;
                foreach(var sibling in rootNode.Parent.Children)
                {
                    if(sibling == rootNode)
                    {
                        foundMe = true;
                        continue;
                    }
                    if (foundMe)
                    {
                        counter++;
                        if (counter >= markerArry.Length)
                        {
                            break;
                        }
                        markerArry[counter] = sibling.Children.Any();
                    }
                }
            }
            foreach(var mark in markerArry)
            {
                if (mark)
                {
                    indentString = indentString +  "|" + filler;
                }
                else
                {
                    indentString = indentString + " " + filler;
                }
            }




            foreach (var n in rootNode.Children)
            {
                var vis = GetVisual(n);
                var line = indentString + "|--- " + vis;
                sb.AppendLine(line);
                DrawVisualRecursive(sb, n);
            }
        }

        //private static void DrawVisualRecursiveOLD(StringBuilder sb, TreeNode<T> rootNode, int level)
        //{
        //    const string filler = "         ";
        //    var tmp = new List<String>();
        //    for (int i = 0; i < level; i++)
        //    {
        //        tmp.Add(filler);
        //    }
        //    var indentString = string.Join('|', tmp); 
        //    if (!string.IsNullOrEmpty(indentString))
        //    {
        //        indentString = "|" + indentString;
        //    }
        //    foreach (var n in rootNode.Children)
        //    {
        //        var vis = GetVisual(n);
        //        var line = indentString + "|--- " + vis;
        //        sb.AppendLine(line);
        //        DrawVisualRecursiveOLD(sb, n, (level + 1));
        //    }
        //}

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