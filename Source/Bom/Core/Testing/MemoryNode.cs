﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.Testing
{
    public class MemoryNode
    {
        public MemoryNode(string title)
        {
            this.Title = title;
        }

        public string Title { get; set; }

        public override string ToString()
        {
            return "" + this.Title;
        }
    }

    public static class MemoryNodeExtensiosn
    {
        public static string GetParentTitle(this TreeNode<MemoryNode> node)
        {
            if (!string.IsNullOrEmpty(node.Data.Title))
            {
                var allParts = node.Data.Title.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (allParts.Length > 1)
                {
                    // parent title is all without the last
                    var allParentParts = allParts.Take(allParts.Length - 1);
                    var parentTitle = string.Join("-", allParentParts);
                    return parentTitle;
                }
            }
            return "";
        }

        public static string GetNonParentPart(this TreeNode<MemoryNode> node)
        {
            if (!string.IsNullOrEmpty(node.Data.Title))
            {
                var allParts = node.Data.Title.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (allParts.Length > 0)
                {
                    // parent title is all without the last
                    var lastPart = allParts.Last();
                    return lastPart;
                }
                return node.Data.Title; // no parent part.. root
            }
            return "";
        }

        public static int GetLevel(this TreeNode<MemoryNode> node)
        {
            var definingPart = GetNonParentPart(node);
            if (!string.IsNullOrEmpty(definingPart))
            {
                var sb = new StringBuilder();
                foreach (var c in definingPart)
                {
                    if (char.IsDigit(c))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        break;
                    }
                }
                var levelPart = sb.ToString();
                var level = int.Parse(levelPart); // will throw if does not comply to convention
                return level;
            }
            return 0;
        }

        public static int GetPosition(this TreeNode<MemoryNode> node)
        {
            var definingPart = GetNonParentPart(node);
            if (!string.IsNullOrWhiteSpace(definingPart))
            {
                char posChar = definingPart.Trim().Last();
                if (char.IsDigit(posChar))
                {
                    throw new InvalidOperationException($"Last char in {definingPart} ({posChar}) is a digit. expected was a letter");
                }
                int index = TestDataFactory.PositionChars.IndexOf(posChar);
                if (index < 0)
                {
                    throw new InvalidOperationException($"index was not found for {posChar} in {TestDataFactory.PositionChars} (defining part: {definingPart})");
                }
                return (index + 1);
            }
            return 0;
        }

        public static IEnumerable<TreeNode<MemoryNode>> GetChildrenByAbsoluteLevel(this IEnumerable<TreeNode<MemoryNode>> treeNodes, int level)
        {
            var nodesOnSpecificLevel = treeNodes.Where(x => x.GetLevel() == level);
            return nodesOnSpecificLevel;
        }

        public static TreeNode<MemoryNode> GetChildNodeByPos(this IEnumerable<TreeNode<MemoryNode>> treeNodes, int pos, int nofSkips = 0)
        {
            var nodesWithPosition = treeNodes.OrderBy(x => x.Data.Title).Where(x => x.GetPosition() == pos);
            if(nofSkips > 0)
            {
                nodesWithPosition.Skip(nofSkips);
            }
            var result = nodesWithPosition.FirstOrDefault();
            return result;
        }
    }
}