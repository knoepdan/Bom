using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Bom.Core.Model;
using Ch.Knomes.Structure;

namespace Bom.Core.Utils
{
    public static class TreeNodeUtils
    {
        public static List<TreeNode<Path>> CreateInMemoryModel(IEnumerable<Path> pathsToCreateTreeFrom)
        {
            // possible improvment.. can probably be done faster (idea..presort by hierarchy in db or here.. )

            if(pathsToCreateTreeFrom == null)
            {
                throw new ArgumentNullException(nameof(pathsToCreateTreeFrom));
            }
            IList<Path> paths;
            if(pathsToCreateTreeFrom is IList<Path>)
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
            foreach(var p in paths.OrderBy(x => x.Level))
            {
                if (checkedPaths.Contains(p))
                {
                    continue;
                }
                checkedPaths.Add(p);

                var treeNode = new TreeNode<Path>(p);
                rootNodes.Add(treeNode);

                // try to find direct children
                AddChildNodesRecursive(treeNode, paths, checkedPaths);
            }
            return rootNodes;
        }

        private static void AddChildNodesRecursive(TreeNode<Path> baseNode, IList<Path> paths, HashSet<Path> checkedPaths )
        {
            var p = baseNode.Data;
            var children = paths.Where(x => x.NodePathString.StartsWith(p.NodePathString) && x.Level == (p.Level + 1) && !checkedPaths.Contains(x));
            foreach (var child in children)
            {
                var childNode = baseNode.AddChild(child);
                checkedPaths.Add(child);
                AddChildNodesRecursive(childNode, paths, checkedPaths);
            }
        }
    }
}
