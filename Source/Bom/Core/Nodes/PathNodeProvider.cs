using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Bom.Core.Common;
using Bom.Core.Nodes.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Bom.Core.Nodes
{
    public class PathNodeProvider
    {
        public ModelContext ModelContext { get; }

        public PathNodeProvider(ModelContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            this.ModelContext = context;
        }

        public Path AddNodeWithPath(Path? parentPath, string nodeTitle)
        {
            // related question
            // https://stackoverflow.com/questions/58207182/how-to-call-a-stored-procedure-in-ef-core-3-0-via-fromsqlraw
            string parentPathString = "";
            if (parentPath != null)
            {
                parentPathString = PathHelper.GetParentPathForChild(parentPath);
                parentPathString = parentPathString.Trim('/');
                // var depth = parentPath.Depth + 1; (depth is set on the database)
            }

            var createdPath = ModelContext.Paths.FromSqlRaw("AddNodeWithPathProc  {0}, {1}", nodeTitle, parentPathString).ToList().First(); // important: first toList() !!!
            return createdPath;
        }

        /// <summary>
        /// Create a new path for an existing node and return new pathId
        /// </summary>
        /// <returns>Id of created path</returns>
        public Path AddPathToNode(Node node, Path? parentPath, bool setAsMainPath = false)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            string parentPathString = "";
            if (parentPath != null)
            {
                parentPathString = PathHelper.GetParentPathForChild(parentPath);
                parentPathString = parentPathString.Trim('/');
            }
            var createdPath = ModelContext.Paths.FromSqlRaw("AddPathToExistingNodeProc  {0}, {1}, {2}", node.NodeId, parentPathString, setAsMainPath).ToList().First(); // important: first toList() !!!
            return createdPath;

        }

        /// <summary>
        /// Moves node (attention.. if childNodes are not to be moved.. the children of the moved node will be moved one level up to the former parent of the moved node
        /// </summary>
        public Path MovePathAndReload(Path pathToMove, int newParentPathId, bool moveChildrenToo)
        {
            return MovePathAndReload(pathToMove, newParentPathId, moveChildrenToo);
        }

        /// <summary>
        /// Moves node (attention.. if childNodes are not to be moved.. the children of the moved node will be moved one level up to the former parent of the moved node
        /// </summary>
        public Path MovePathAndReload(int pathIdToMove, int newParentPathId, bool moveChildrenToo)
        {
            if(this.ModelContext == null)
            {
                throw new InvalidOperationException($"{nameof(this.ModelContext)} is null. Cannot operate");
            }
            if (this.ModelContext.Paths == null)
            {
                throw new InvalidOperationException($"{nameof(this.ModelContext)}.{nameof(this.ModelContext.Paths)} is null. Cannot execute function");
            }
            MovePath(pathIdToMove, newParentPathId, moveChildrenToo);
            var movedPath = this.ModelContext.Paths.Single(p => p != null && p.PathId == pathIdToMove);
            ModelContext.Entry(movedPath).Reload();// needed
            return movedPath;
        }

        public void MovePath(int pathIdToMove, int newParentPathId, bool moveChildrenToo)
        {
            var spParams = new object[] { pathIdToMove, newParentPathId, moveChildrenToo };
            this.ModelContext.ExecuteRawSql("EXEC MoveNodeProc @p0, @p1, @p2", spParams);
            // SP
            //CREATE PROCEDURE dbo.[MoveNodeProc]   @pathId INT NULL,	@newParentPathId INT NULL,	@moveChildrenToo BIT NULL
        }

        public void DeletePath(Path pathToDelete, bool alsoDeleteNode, bool alsoDeleteSubTree = false)
        {
            if (pathToDelete == null)
            {
                throw new ArgumentNullException(nameof(pathToDelete));
            }
            DeletePath(pathToDelete.PathId, alsoDeleteNode, alsoDeleteSubTree);
        }

        public void DeletePath(int pathIdToDelete, bool alsoDeleteNode, bool alsoDeleteSubTree)
        {
            object[] spParams;

            var p0 = pathIdToDelete;
            var p1 = alsoDeleteNode;
            var p2 = alsoDeleteSubTree;
            spParams = new object[] { p0, p1, p2 };
            this.ModelContext.ExecuteRawSql("EXEC DeletePathProc @p0, @p1, @p2", spParams);

            // SP
            //CREATE PROCEDURE dbo.[DeletePathProc] @pathId INT NULL, @deleteSubTree AS BIT = 0, @alsoDeleteNode AS BIT = 0
        }



        #region old outcommented

        //public void DeletePath(Path pathToDelete, bool alsoDeleteNode, int? newMainPathId)
        //{
        //    DeletePath(pathToDelete.PathId, alsoDeleteNode, newMainPathId);
        //}

        //public void DeletePath(int pathIdToDelete, bool alsoDeleteNode, int? newMainPathId)
        //{
        //    object[] spParams;
        //    var p0 = new System.Data.SqlClient.SqlParameter("@p0", pathIdToDelete);
        //    var p1 = new System.Data.SqlClient.SqlParameter("@p1", alsoDeleteNode);
        //    if (newMainPathId.HasValue)
        //    {
        //        spParams = new object[] { p0, p1, new System.Data.SqlClient.SqlParameter("@p2", newMainPathId.Value) };
        //        this.ModelContext.ExecuteRawSql("EXEC DeletePathProc @p0, @p1, @p2", spParams);
        //    }
        //    else
        //    {
        //        spParams = new object[] { p0, p1 };
        //        this.ModelContext.ExecuteRawSql("EXEC DeletePathProc @p0, @p1", spParams);
        //    }

        //    // SP
        //    //CREATE PROCEDURE dbo.[DeletePathProc] @pathId INT NULL, @newMainPathId INT NULL, @alsoDeleteNode BIT  NULL
        //}

        #endregion
    }
}