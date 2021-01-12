using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Nodes.DbModels;
using System.Globalization;

namespace Bom.Core.Nodes
{
    public static class PathQueries
    {

        public static IQueryable<Path> Descendants(this IQueryable<Path>? list, Path basePath, int? childDepth = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }
#pragma warning disable CA1310 // Specify StringComparison for correctness
            list = list.Where(x => x.NodePathString.StartsWith(basePath.NodePathString) && x.Level > basePath.Level);
#pragma warning restore CA1310 // Specify StringComparison for correctness
            if (childDepth.HasValue)
            {
                // limit depth (example: basePath.Depth is 4, childDepth 2 then depth as big as 6
                int maxDepth = (basePath.Level + childDepth.Value);
                list = list.Where(x => x.Level <= maxDepth);

            }
            return list;
        }

        public static IQueryable<Path> Siblings(this IQueryable<Path>? list, Path basePath)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (basePath.IsRoot())
            {
                // is root
                if (basePath.Level != 1)
                {
                    throw new ArgumentException($"path has no PathValues but an unexpected depth {basePath.Level}"); // data inconsistency (should not happen.. maybe solve on a different level)
                }

                list = list.AllRootPaths();// Where(x => string.IsNullOrEmpty(x.ParentPath) && x.Depth == 0); // all roots
            }
            else
            {
                var pathOfDirectParent = basePath.GetParentPath(1);
#pragma warning disable CA1310 // Specify StringComparison for correctness
                list = list.Where(x => x.NodePathString.StartsWith(pathOfDirectParent) && x.Level == basePath.Level && x.PathId != basePath.PathId);
#pragma warning restore CA1310 // Specify StringComparison
            }
            return list;
        }

        public static Path? DirectParent(this IQueryable<Path>? list, Path basePath)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (basePath.IsRoot())
            {
                // root
                return null;//list.FirstOrDefault(x => x.PathId == 0 && x.ParentPath == "dkls" && x.Depth == -99); // will return null
            }

            // actual search for parent
            var pathOfDirectParent = basePath.GetParentPath(1);
#pragma warning disable CA1310 // Specify StringComparison for correctness
            var parent = list.FirstOrDefault(x => x.NodePathString.StartsWith(pathOfDirectParent) && x.Level < basePath.Level);
#pragma warning restore CA1310 // Specify StringComparison
            return parent;
        }

        public static IQueryable<Path> Ancestors(this IQueryable<Path>? list, Path basePath, int? stepsToGoUp = null, bool orderByDepth = true)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (basePath.IsRoot())
            {
                // root
                return list.Where(x => x.PathId == 0 && x.NodePathString == "dkls" && x.Level == -99); // will return an empty list
            }

            // actual search for parent
            var allPaths = PathHelper.GetAllParentPaths(basePath, stepsToGoUp);
            list = list.Where(p => allPaths.Contains(p.NodePathString) && p.Level < basePath.Level); // possible improvments: 1. sql server may not be able to handle too many where params 2. possible performance improvments when including path somehow

            if (orderByDepth)
            {
                list = list.OrderBy(x => x.Level);
            }
            return list;
        }

        public static IQueryable<Path> AllRootPaths(this IQueryable<Path>? list)
        {
            if(list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var rootElements = list.Where(x => x.Level == 1);
            return rootElements;
        }
    }
}
