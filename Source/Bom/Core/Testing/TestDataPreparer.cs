using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Core.Data;
using Core.Model;
using Core.DataAccess;

namespace Core.Testing
{
    public class TestDataPreparer
    {
        private readonly ModelContext _context;

        private readonly PathNodeProvider _pathNodeProvider;

        public TestDataPreparer(ModelContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            _context = context;
            _pathNodeProvider = new PathNodeProvider(this._context);
        }

        public void CleanTestDatabase()
        {
            this._context.ExecuteRawSql("EXEC ClearDatabaseProc");
            // code below will not work due to a closed connection
            //using (var cmd = this._context.GetStoredProcedureCommand("ClearDatabaseProc"))
            //{
            //    var result = cmd.ExecuteNonQuery();
            //}
        }

        public Path CreateTestData(TreeNode<MemoryNode> rootNode)
        {
            CleanTestDatabase();

            // insert data
            return InsertNodeAndAllChildren(rootNode, null);
        }

        private Path InsertNodeAndAllChildren(TreeNode<MemoryNode> rootNode, Path dbParentPath = null)
        {
            var rootPath = AddNode(rootNode.Data.Title, dbParentPath);
            foreach (var child in rootNode.Children)
            {
                InsertNodeAndAllChildren(child, rootPath);
            }
            return rootPath;
        }

        private Path AddNode(string nodeTitle, Path parentPath, bool saveOnAdd = true)
        {
            var path = this._pathNodeProvider.AddNodeWithPath(parentPath, nodeTitle);
            if (saveOnAdd)
            {
                this._pathNodeProvider.ModelContext.SaveChanges();
            }
            return path;
        }
    }
}