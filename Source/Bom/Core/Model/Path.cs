using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Bom.Utils.Math;
using Bom.Core.DataAccess;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using Bom.Core.Utils;
using Ch.Knomes.Structure;

namespace Bom.Core.Model
{
    public class Path : ITreeNodeTitle
    {
        public const char Separator = '/';

        public int PathId { get; protected set; }

        internal SqlHierarchyId NodePath { get; set; }

        public short Level { get; protected set; }

        public string NodePathString { get; protected set; }

        public int NodeId { get; internal set; }

        public virtual Node Node { get; internal set; }

        #region calculated values

        /// <summary>
        /// NodeIds as string (not yet converted)
        /// </summary>
        internal string[] AllRawNodeIds => PathHelper.GetPathValues(NodePathString);

        public IEnumerable<long> AllNodeIds => PathHelper.GetNodeIdsFromPath(NodePathString);

        internal int NofFragments => AllRawNodeIds != null ? AllRawNodeIds.Length : 0;


        #endregion

        string ITreeNodeTitle.GetTitleString()
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
