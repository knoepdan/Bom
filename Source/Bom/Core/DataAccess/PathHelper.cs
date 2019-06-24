using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Core.Model;
using Utils.Math;

namespace Core.DataAccess
{
    public static class PathHelper
    {

        public static IEnumerable<string> GetParentPathFragments(this Path path, int stepsToGoUp)
        {
            // 35/36/45/46/47/48
            // steps to go up == 2 ->  35/36/45/46
            if (path.IsRoot())
            {
                return new string[0]; // there is nothing above
            }
            var tmp = path.PathValues;
            var steps = stepsToGoUp + 1; // as current node is also in path
            if (tmp.Length > steps)
            {
                var nofSteps = tmp.Length - steps + 1;
                var relevantPaths = tmp.Take(nofSteps);
                return relevantPaths;
            }else if(tmp.Length > 0)
            {
                return tmp.Take(1); // return root as we cannot go any further
            }

            throw new InvalidOperationException($"Cannot get parent fragments from '{path.NodePathString}' steps to go up: {stepsToGoUp}"); // impossible
        }

        public static IEnumerable<string> GetAllParentPaths(this Path path, int? stepsToGoUp)
        {
            if (path.IsRoot())
            {
                return new List<string>(); // root
            }
            var tmp = path.PathValues;
            var listOfParentPaths = new List<string>();

            int steps = stepsToGoUp.HasValue ? Math.Min(stepsToGoUp.Value, tmp.Length) : tmp.Length;
            for(int i = 0; i < tmp.Length; i++)
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
            var tmp = path.PathValues;
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
            if (path.PathValues != null)
            {
                fragments.AddRange(path.PathValues);
            }
            var finalPath = CreatePathFromFragments(fragments);
            return finalPath;
        }

        private static IEnumerable<long> GetNodeIdsFromPathValues(IEnumerable<string> pathValues)
        {
            if (pathValues != null)
            {
                var val = pathValues.Select(x => (long.Parse(x)));//pathValues.Select(x => NumericalSystemHelper.ArbitraryToDecimalSystem(x));
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
            return new string[0];
        }

        private static string CreatePathFromFragments(IEnumerable<string> pathValues)
        {
            var pathString = Path.Separator + string.Join(Path.Separator, pathValues) + Path.Separator;
            return pathString;
        }

        public static bool IsRoot(this Path path)
        {
            if(path.Level == 0)
            {
                return true;
            }
            return false;
        }

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
