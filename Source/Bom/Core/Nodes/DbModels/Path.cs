using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Bom.Utils.Math;
using Microsoft.SqlServer.Server;
using Bom.Core.Utils;
using Ch.Knomes.Struct;

namespace Bom.Core.Nodes.DbModels
{
    public class Path : ITreeNodeTitle
    {
        public const char Separator = '/';

        public int PathId { get; protected set; }

        public short Level { get; protected set; }

        public string NodePathString { get; protected set; } = "";

        public int NodeId { get; internal protected set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public virtual Node Node { get; internal protected set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.


        /// -> outcommented because not working in .net core 3.0 
        //internal Microsoft.SqlServer.Types.SqlHierarchyId NodePath { get; set; }

        #region calculated values

        /// <summary>
        /// NodeIds as string (not yet converted)
        /// </summary>
        internal string[] AllRawNodeIds => PathHelper.GetPathValues(NodePathString);

        public IEnumerable<long> AllNodeIds => PathHelper.GetNodeIdsFromPath(NodePathString);

        internal int NofFragments => AllRawNodeIds != null ? AllRawNodeIds.Length : 0;


        #endregion

#pragma warning disable CA1033 // Interface methods should be callable by child types
        string ITreeNodeTitle.GetTitleString()
#pragma warning restore CA1033 // Interface methods should be callable by child types
        {
            var node = $"{PathId} '{string.Join(Path.Separator, this.AllNodeIds)}'";
#if DEBUG
            if (Node != null)
            {
                node += $" (Node: '{Node.Title}')";
            }
#endif
            return node;
        }


        public override string ToString()
        {
            var node = $"Path {PathId} '{string.Join(Path.Separator, this.AllNodeIds)}'";
#if DEBUG
            if (Node != null)
            {
                node += $" (Node: '{Node.Title}')";
            }
#endif
            return node;
        }

        #region basic tests

        static Path()
        {
#if DEBUG
            PerformInternalTests();
#endif
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private static void PerformInternalTests()
        {
            var errorMessages = new List<string>();

            // 1. GetPathValues(string pathString)
            var path = new Path();
            path.NodePathString = "/44/2/7/";
            var childPath = PathHelper.GetParentPathForChild(path); // not possible to test in 
            if(childPath != "/44/2/7/")
            {
                errorMessages.Add($"{nameof(PathHelper)}.{nameof(PathHelper.GetPathValues)} does not return expected results");
            }

            //  summary
            if (errorMessages.Count > 0)
            {
                throw new Exception($"Not all tests were successful: {string.Join(", ", errorMessages)}");
            }
        }


        #endregion

        #region outcommented stuff


        //public PathStuff MorePathInfo { get; }

        ///// <summary>
        ///// Indicates the depth in the tree (Root Nodes have Depth 0)
        ///// </summary>
        //[Obsolete("replaced by Hierary")]
        //protected internal int SetDepth { get; set; }

        //[Obsolete("replaced Depth")]
        //protected internal string SetParentPath { get; set; }


        //public class PathStuff
        //{
        //    public PathStuff(Path p)
        //    {
        //        this.Path = p;
        //    }

        //    internal Path Path {get;}

        //    public string ParentPath => this.Path.SetParentPath;

        //    public int SetDepth => this.Path.SetDepth;

        //    public string HierarchyNetString => this.Path.Hierarchy.ToString();
        //}

        #endregion
    }
}
