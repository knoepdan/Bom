﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Bom.Core.Nodes.DbModels;

namespace Bom.Core.Nodes
{
    public static class PathHelper
    {
        static PathHelper()
        {
#if DEBUG
            PerformInternalTests();
#endif
        }

        public static IEnumerable<string> GetParentPathFragments(this Path path, int stepsToGoUp)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // 35/36/45/46/47/48
            // steps to go up == 2 ->  35/36/45/46
            if (path.IsRoot())
            {
                return Array.Empty<string>(); // there is nothing above
            }
            var tmp = path.AllRawNodeIds;
            var steps = stepsToGoUp + 1; // as current node is also in path
            if (tmp.Length > steps)
            {
                var nofSteps = tmp.Length - steps + 1;
                var relevantPaths = tmp.Take(nofSteps);
                return relevantPaths;
            }
            else if (tmp.Length > 0)
            {
                return tmp.Take(1); // return root as we cannot go any further
            }

            throw new InvalidOperationException($"Cannot get parent fragments from '{path.NodePathString}' steps to go up: {stepsToGoUp}"); // impossible
        }

        public static IEnumerable<string> GetAllParentPaths(this Path path, int? stepsToGoUp)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.IsRoot())
            {
                return new List<string>(); // root
            }
            var tmp = path.AllRawNodeIds;
            var listOfParentPaths = new List<string>();

            int steps = stepsToGoUp.HasValue ? Math.Min(stepsToGoUp.Value, tmp.Length - 1) : tmp.Length - 1;
            for (int i = 1; i <= steps; i++)
            {
                var parentPath = GetParentPathFragments(path, i);
                var parentPathString = CreatePathFromFragments(parentPath); // would be something like this: string.Join(Path.Separator, pathValues);
                listOfParentPaths.Add(parentPathString);
            }
            return listOfParentPaths;
        }

        public static string GetParentPath(this Path path, int stepsToGoUp)
        {
            if (path.IsRoot())
            {
                return ""; // root
            }
            var tmp = path.AllRawNodeIds;
            var pathValues = GetParentPathFragments(path, stepsToGoUp);
            var pathString = CreatePathFromFragments(pathValues); // would be something like this: string.Join(Path.Separator, pathValues);
            return pathString;
        }

        public static IEnumerable<long> GetNodeIdsFromPath(string pathString)
        {
            var pathValues = GetPathValues(pathString);
            var result = GetNodeIdsFromPathValues(pathValues);
            return result;
        }

        internal static string GetParentPathForChild(Path path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            var fragments = new List<string>();
            if (path.AllRawNodeIds != null)
            {
                fragments.AddRange(path.AllRawNodeIds);
            }
            var finalPath = CreatePathFromFragments(fragments);
            return finalPath;
        }

        private static IEnumerable<long> GetNodeIdsFromPathValues(IEnumerable<string> pathValues)
        {
            if (pathValues != null)
            {
                var val = pathValues.Select(x => (long.Parse(x, System.Globalization.CultureInfo.InvariantCulture)));//pathValues.Select(x => NumericalSystemHelper.ArbitraryToDecimalSystem(x));
                return val;
            }
            return new List<long>();
        }

        internal static string[] GetPathValues(string pathString)
        {
            if (!string.IsNullOrEmpty(pathString))
            {
                var tmp = pathString.Split(Path.Separator, StringSplitOptions.RemoveEmptyEntries);
                return tmp;
            }
            return Array.Empty<string>();
        }

        private static string CreatePathFromFragments(IEnumerable<string> pathValues)
        {
            var pathString = Path.Separator + string.Join(Path.Separator, pathValues) + Path.Separator;
            return pathString;
        }

        public static bool IsRoot(this Path path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.Level <= 1)
            {
                return true;
            }
            return false;
        }

        #region tests for non public methods (basic as public methods are to be unit tested)


        [System.Diagnostics.Conditional("DEBUG")]
        private static void PerformInternalTests()
        {
            // some basic tests
            var errorMessages = new List<string>();
            
            // 1. GetPathValues(string pathString)
            var pathArray = GetPathValues("a/b/");
            if(pathArray.Length != 2 || pathArray[0] != "a" || pathArray[1] != "b")
            {
                errorMessages.Add($"{nameof(GetPathValues)} does not return the expected result");
            }

            //  2. CreatePathFromFragments(IEnumerable<string> pathValues)
            if (CreatePathFromFragments(new[] { "1", "b"}) != "/1/b/" || CreatePathFromFragments(new[] { "x" }) != "/x/")
            {
                errorMessages.Add($"{nameof(CreatePathFromFragments)} does not return the expected result");
            }
            //  3. GetNodeIdsFromPathValues(IEnumerable<string> pathValues)
            var pathValues = GetNodeIdsFromPathValues(new[] { "23", "99" }).ToList();
            if (pathValues.Count != 2 || pathValues[0] != 23 || pathValues[1] != 99)
            {
                errorMessages.Add($"{nameof(GetNodeIdsFromPathValues)} does not return the expected result");
            }
            //  string GetParentPathForChild(Path path) -> cannot be tested here
            if(errorMessages.Count > 0)
            {
                throw new Exception($"Not all tests were successful: {string.Join(", ", errorMessages)}");
            }
        }
        
        #endregion

        #region not used
        //public static IEnumerable<long> GetParentNodeIds(this Path path, int? stepsToGoUp = null)
        //{
        //    if (!stepsToGoUp.HasValue)
        //    {
        //        // entire graph
        //        var allParents = path.AllParentNodeIds; // PathHelper.GetPathIdsFromPathValues(path.ParentPathValues);
        //        return allParents;
        //    }
        //    var stringFragments = GetParentPathFragments(path, stepsToGoUp.Value);
        //    var result = GetNodeIdsFromPathValues(stringFragments);
        //    return result;
        //}

        #endregion
    }
}
