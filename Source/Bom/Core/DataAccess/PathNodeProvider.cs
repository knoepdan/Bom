using System;
using System.Collections.Generic;
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
            if(context == null)
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
    }
}