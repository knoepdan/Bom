using System;
using System.Linq;
using System.Collections.Generic;
using Bom.Core.Nodes.DbModels;
using Ch.Knomes.Struct;

namespace Bom.Core.Utils
{
    public static class TreeNodeUtils
    {
        public static List<TreeNode<Path>> CreateInMemoryModel(IEnumerable<Path> pathsToCreateTreeFrom, char pathSeparator = '/')
        {
            // possible improvment.. can probably be done faster (idea..presort by hierarchy in db or here.. )

            if (pathsToCreateTreeFrom == null)
            {
                throw new ArgumentNullException(nameof(pathsToCreateTreeFrom));
            }
            IList<Path> paths;
            if (pathsToCreateTreeFrom is IList<Path>)
            {
                paths = (IList<Path>)pathsToCreateTreeFrom;
            }
            else
            {
                paths = pathsToCreateTreeFrom.ToList();
            }

            // create tree
            var rootNodes = new List<TreeNode<Path>>(); // attention: any node that has no direct parent in the passed list is considered a root node (subsubchildren where the parent is not passed, are considered root nodes)
            var checkedPaths = new HashSet<Path>();
            foreach (var p in paths.OrderBy(x => x.Level))
            {
                if (checkedPaths.Contains(p))
                {
                    continue;
                }
                checkedPaths.Add(p);

                var treeNode = new TreeNode<Path>(p);
                rootNodes.Add(treeNode);

                // try to find direct children
                AddChildNodesRecursive(treeNode, paths, checkedPaths, pathSeparator);
            }
            return rootNodes;
        }

        private static void AddChildNodesRecursive(TreeNode<Path> baseNode, IList<Path> paths, HashSet<Path> checkedPaths, char pathSeparator)
        {
            var p = baseNode.Data;
            if (string.IsNullOrEmpty(p.NodePathString))
            {
                return;
            }

            var pathToCheck = p.NodePathString.TrimEnd(pathSeparator) + pathSeparator;
            var children = paths.Where(x => x.NodePathString.StartsWith(pathToCheck, StringComparison.InvariantCulture) && x.Level == (p.Level + 1) && !checkedPaths.Contains(x));
            foreach (var child in children)
            {
                var childNode = baseNode.AddChild(child);
                checkedPaths.Add(child);
                AddChildNodesRecursive(childNode, paths, checkedPaths, pathSeparator);
            }
        }

        #region faster way (not yet finalized)
        /*

        /// <summary>
        /// Creates an in memory model from denormalized nodes (eg. denormalized path: "/23/323/32/")
        /// ATTENTION: path segments may not contain characters that are before and after the separator character!!
        /// Example: segment: "/2%32/" contains '%' which in sorting will be before the separator '/'. This will confuse the sorting and lead to incorrect results.
        /// (with the default separator, digits and letters are ok)
        /// </summary>
        /// <param name="elementsSortedByTreePath">objects with a denormalized TreePath, sorted by Treepath</param>
        /// <param name="pathSeparator">path separator default is '/'</param>
        /// <returns>tree object, multiple objects if there is more than one tree path</returns>
        public static List<TreeNode<T>> CreateInMemoryModelFromSortedNodes<T>(IEnumerable<T> elementsSortedByTreePath, char pathSeparator = '/') where T : class, IDenormalizedTreeNode
        {
            if (elementsSortedByTreePath == null)
            {
                throw new ArgumentNullException(nameof(elementsSortedByTreePath));
            }
            IList<T> paths;
            if (elementsSortedByTreePath is IList<T>)
            {
                paths = (IList<T>)elementsSortedByTreePath;
            }
            else
            {
                paths = elementsSortedByTreePath.ToList();
            }

            var rootNodes = new List<TreeNode<T>>(); // attention: any node that has no direct parent in the passed list is considered a root node (subsubchildren where the parent is not passed, are considered root nodes)

            TreeNode<T> lastNode = null;
            for (int i = 0; i < paths.Count; i++)
            {
                var p = paths[i];

                // try to find parent node
                TreeNode<T> treeNode = null;
                if (lastNode == null)
                {
                    treeNode = new TreeNode<T>(p);
                    rootNodes.Add(treeNode);
                }
                else
                {
                    // 3 possibilities: is parent, is sibling, this is a root node a last item belongs to a different tree
                    if (IsChildOrDescendant(lastNode.Data, p, pathSeparator))
                    {
                        treeNode = lastNode.AddChild(p);
                    }
                    else if (lastNode.Parent == null)
                    {
                        // new root because: not a child and last one is no root
                        treeNode = new TreeNode<T>(p);
                        rootNodes.Add(treeNode);
                    }
                    else
                    {
                        bool parentFound = false;

                        var ancestors = lastNode.Ancestors; // is sorted by proximity to current node (parent is first in list)
                        foreach (var potentialParent in ancestors)
                        {
                            if (IsChildOrDescendant(potentialParent.Data, p, pathSeparator))
                            {
                                // yes is sibling
                                treeNode = potentialParent.AddChild(p);
                                parentFound = true;
                                break;
                            }
                        }
                        if (!parentFound)
                        {
                            // new root because: not a child and last one is no root
                            treeNode = new TreeNode<T>(p);
                            rootNodes.Add(treeNode);
                        }
                    }
                }
                lastNode = treeNode;
            }
            return rootNodes;
        }

        private static bool IsChildOrDescendant(IDenormalizedTreeNode parent, IDenormalizedTreeNode potentialSubnode, char separator)
        {
            var subnodePath = potentialSubnode.TreePath.TrimEnd(separator) + separator;
            var parentPath = parent.TreePath.TrimEnd(separator) + separator;
            if (subnodePath.StartsWith(parentPath, StringComparison.InvariantCulture) && subnodePath.Length > parentPath.Length)
            {
                return true;
            }
            return false;
        }
        */
        #endregion
    }
}
