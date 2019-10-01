﻿using System;
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

        public Path AddNodeWithPath(Path parentPath, string nodeTitle)
        {
            // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx

            string parentPathString = "";
            if (parentPath != null)
            {
                parentPathString = PathHelper.GetParentPathForChild(parentPath);
                parentPathString = parentPathString.Trim('/');
                // var depth = parentPath.Depth + 1; (depth is set on the database)
            }

            var spParams = new object[] { nodeTitle, parentPathString };
            // var createdPath = ModelContext.Paths.FromSql("AddNodeWithPathProc  @p0, @p1", spParams).Single();
         //    this.ModelContext.ExecuteRawSql("EXEC AddNodeWithPathProc @p0, @p1", spParams); -> works but no return value
            //      var createdPath = ModelContext.Paths.FromSqlRaw("AddNodeWithPathProc @p0, @p1", spParams).Single();




            var p0 = new System.Data.SqlClient.SqlParameter("@title", nodeTitle);
          //  p0.Value = nodeTitle;
            var p1 = new System.Data.SqlClient.SqlParameter("@parentPath", parentPathString);
            //p1.Value = "asdfsdf";
            //    var createdPath =   ModelContext.Paths.FromSqlRaw("EXEC AddNodeWithPathProc @title, @parentPath", p0, p1).Single();

            Path createdPath = null;
            try
            {
                createdPath = ModelContext.Paths.FromSqlRaw("EXECUTE AddNodeWithPathProc {0}, {1}", nodeTitle, parentPathString).Single();
//                exec sp_executesql N'SELECT TOP(2) [p].[PathId], [p].[Level], [p].[NodeId], [p].[NodePath], [p].[NodePathString]
//FROM(
//    EXEC AddNodeWithPathProc @p0, @p1
//) AS[p]',N'@p0 nvarchar(4000),@p1 nvarchar(4000)',@p0=N'1a',@p1=N''


            }
            catch (Exception exxx)
            {
                try
                {
                    // will fail bfore sending query to database
                    createdPath = ModelContext.Paths.FromSqlRaw("EXECUTE AddNodeWithPathProc {0}, {1}", p0, p1).Single();
                }catch(Exception xxxxx)
                {
                    // will fail too (and send query to db)
                    createdPath = ModelContext.Paths.FromSqlRaw("AddNodeWithPathProc @p0, @p1", spParams).Single();
                }
            }

            //    var createdPath = ModelContext.Paths.FromSql("AddNodeWithPathProc  @p0, @p1", spParams).Single();
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
            DeletePath(pathToDelete.PathId, alsoDeleteNode, alsoDeleteSubTree);
        }

        public void DeletePath(int pathIdToDelete, bool alsoDeleteNode, bool alsoDeleteSubTree)
        {
            object[] spParams;

            //var p0 = new System.Data.SqlClient.SqlParameter("@p0", pathIdToDelete);
            //var p1 = new System.Data.SqlClient.SqlParameter("@p1", alsoDeleteNode);
            //var p2 = new System.Data.SqlClient.SqlParameter("@p2", alsoDeleteSubTree);
            var p0 = pathIdToDelete; // new System.Data.SqlClient.SqlParameter("@p0", pathIdToDelete);
            var p1 = alsoDeleteNode; // new System.Data.SqlClient.SqlParameter("@p1", alsoDeleteNode);
            var p2 = alsoDeleteSubTree; // new System.Data.SqlClient.SqlParameter("@p2", alsoDeleteSubTree);
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