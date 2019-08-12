using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Core.Data;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Core.DataAccess
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
            var createdPath = ModelContext.Paths.FromSql("AddNodeWithParentPathProc  @p0, @p1", spParams).Single();
            return createdPath;
        }

        /// <summary>
        /// Moves node (attention.. if childNodes are not to be moved.. the children of the moved node will be moved one level up to the former parent of the moved node
        /// </summary>
        public Path MovePath(Path pathToMove, int newParentPathId, bool moveChildrenToo)
        {
            return MovePath(pathToMove, newParentPathId, moveChildrenToo);
        }

        /// <summary>
        /// Moves node (attention.. if childNodes are not to be moved.. the children of the moved node will be moved one level up to the former parent of the moved node
        /// </summary>
        public Path MovePath(int pathIdToMove, int newParentPathId, bool moveChildrenToo)
        {
            var spParams = new object[] { pathIdToMove, newParentPathId, moveChildrenToo };
            var movedPath = ModelContext.Paths.FromSql("MoveNodeProc  @p0, @p1, @p2", spParams).Single();
            return movedPath;

            // SP
            //CREATE PROCEDURE dbo.[MoveNodeProc]   @pathId INT NULL,	@newParentPathId INT NULL,	@moveChildrenToo BIT NULL
        }

        public void DeletePath(Path pathToDelete, bool alsoDeleteNode, int? newMainPathId)
        {
            DeletePath(pathToDelete.PathId, alsoDeleteNode, newMainPathId);
        }

        public void DeletePath(int pathIdToDelete, bool alsoDeleteNode, int? newMainPathId)
        {
            object[] spParams;
            var p0 = new System.Data.SqlClient.SqlParameter("@p0", pathIdToDelete);
            var p1 = new System.Data.SqlClient.SqlParameter("@p1", alsoDeleteNode);
            if (newMainPathId.HasValue)
            {
                spParams = new object[] {p0 , p1, new System.Data.SqlClient.SqlParameter("@p2", newMainPathId.Value) };
                this.ModelContext.ExecuteRawSql("EXEC DeletePathProc @p0, @p1, @p2", spParams);
            }
            else
            {
                spParams = new object[] { p0, p1 };
                this.ModelContext.ExecuteRawSql("EXEC DeletePathProc @p0, @p1", spParams);
            }

            // SP
            //CREATE PROCEDURE dbo.[DeletePathProc] @pathId INT NULL, @newMainPathId INT NULL, @alsoDeleteNode BIT  NULL
        }
    }
}