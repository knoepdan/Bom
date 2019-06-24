using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Utils.Math;
using Core.DataAccess;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

namespace Core.Model
{
    public class Path
    {
        public const char Separator = '/';

        public int PathId { get; protected set; }

        internal SqlHierarchyId NodePath { get; set; }

        public short Level { get; protected set; }

        public string NodePathString { get; protected set; }
        
        public int NodeId { get; internal set; }

        public virtual Node Node { get; internal set; }

        #region calculated values

        public string[] PathValues => PathHelper.GetPathValues(NodePathString);

        public int NofFragments => PathValues != null ? PathValues.Length : 0;

        public IEnumerable<long> AllNodeIds => PathHelper.GetNodeIdsFromPath(NodePathString);

        public IEnumerable<long> AllParentNodeIds
        {
            get
            {
                var allNodeIds = AllNodeIds;
                if (allNodeIds.Any())
                {
                    allNodeIds.Skip(1);
                }
                return new List<long>();
            }
        }

        #endregion


        public override string ToString()
        {
            var node = $"Path {PathId} '{string.Join(Path.Separator, this.AllParentNodeIds)}'";
#if DEBUG
            if (Node != null)
            {
                node += $" (Node: '{Node.Title}')";
            }
#endif
            return node;
        }


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
