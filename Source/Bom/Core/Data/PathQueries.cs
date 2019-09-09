using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Model;
using System.Globalization;

namespace Bom.Core.Data
{
    public static class PathQueries
    {

        public static IQueryable<Path> GetChildren(this IQueryable<Path> list, Path basePath, int? childDepth = null)
        {
            list = list.Where(x => x.NodePathString.StartsWith(basePath.NodePathString) && x.Level > basePath.Level);
            if (childDepth.HasValue)
            {
                // limit depth (example: basePath.Depth is 4, childDepth 2 then depth as big as 6
                int maxDepth = (basePath.Level + childDepth.Value);
                list = list.Where(x => x.Level <= maxDepth);

            }
            return list;
        }

        public static IQueryable<Path> GetSiblings(this IQueryable<Path> list, Path basePath)
        {
            if (basePath.IsRoot())
            {
                // is root
                if (basePath.Level != 1)
                {
                    throw new ArgumentException($"path has no PathValues but an unexpected depth {basePath.Level}"); // data inconsistency (should not happen.. maybe solve on a different level)
                }

                list = list.GetRootElements();// Where(x => string.IsNullOrEmpty(x.ParentPath) && x.Depth == 0); // all roots
            }
            else
            {
                var pathOfDirectParent = basePath.GetParentPath(1);
                list = list.Where(x => x.NodePathString.StartsWith(pathOfDirectParent) && x.Level == basePath.Level);
            }
            return list;
        }

        public static Path GetDirectParent(this IQueryable<Path> list, Path basePath)
        {
            if (basePath.IsRoot())
            {
                // root
                return null;//list.FirstOrDefault(x => x.PathId == 0 && x.ParentPath == "dkls" && x.Depth == -99); // will return null
            }

            // actual search for parent
            var pathOfDirectParent = basePath.GetParentPath(1);
            var parent = list.FirstOrDefault(x => x.NodePathString.StartsWith(pathOfDirectParent) && x.Level < basePath.Level);
            return parent;
        }

        public static IQueryable<Path> GetParents(this IQueryable<Path> list, Path basePath, int? stepsToGoUp = null, bool orderByDepth = true)
        {
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

        public static IQueryable<Path> GetRootElements(this IQueryable<Path> list)
        {
            var rootElements = list.Where(x => x.Level == 1);
            return rootElements;
        }
    }
}
