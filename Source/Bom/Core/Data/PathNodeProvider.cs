using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Bom.Core.Data;
using Bom.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Bom.Core.Data
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


        public Path AddPathToNodeAndGetNewPath(Node node, Path? parentPath, bool setAsMainPath = false)
        {
            int newPathId = AddPathToNode(node, parentPath, setAsMainPath);
            var newPath = this.ModelContext.GetPaths().Single(x => x.PathId == newPathId);
            return newPath;
        }

        public int AddPathToNode(Node node, Path? parentPath, bool setAsMainPath = false)
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

            // how to handle output params: https://stackoverflow.com/questions/45252959/entity-framework-core-using-stored-procedure-with-output-parameters
            var nodeIdParam = new Microsoft.Data.SqlClient.SqlParameter("@p0", node.NodeId);
            var parentPathStringParam = new Microsoft.Data.SqlClient.SqlParameter("@p1", parentPathString);
            var setAsMainParam = new Microsoft.Data.SqlClient.SqlParameter("@p2", setAsMainPath);
            var outPath = new Microsoft.Data.SqlClient.SqlParameter("@outPath", SqlDbType.Int);

            this.ModelContext.ExecuteRawSql("EXEC AddPathProc @p0, @p1, @p2, @ParamOut2 OUT", nodeIdParam, parentPathStringParam, setAsMainParam, outPath);

            var newPathId = (int)outPath.Value;
            return newPathId;

            //var outPath = new Microsoft.Data.SqlClient.SqlParameter("@outPath", SqlDbType.Int);
            //var spParams = new object[] { node.NodeId, parentPathString, setAsMainPath };
            //this.ModelContext.ExecuteRawSql("EXEC AddPathProc @p0, @p1, @p2", spParams);

            // probably not working as we have an output param
            //    var createdPath = ModelContext.Paths.FromSqlRaw("AddPathProc  {0}, {1}", node.NodeId, parentPathString, setAsMainPath).ToList().First(); // important: first toList() !!!
            //  return createdPath;

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
            MovePath(pathIdToMove, newParentPathId, moveChildrenToo);
            var movedPath = this.ModelContext.GetPaths().Single(p => p.PathId == pathIdToMove);
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