using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Bom.Core.Common;
using Bom.Core.Nodes;
using Bom.Core.Nodes.DbModels;
using Bom.Core.Utils;
using Ch.Knomes.Struct;

namespace Bom.Core.Testing
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

        public Path CreateTestData(TreeNode<SimpleNode> rootNode, bool cleanDatabase = true)
        {
            if(rootNode == null)
            {
                throw new ArgumentNullException(nameof(rootNode));
            }
            if (cleanDatabase)
            {
                CleanTestDatabase();
            }

            // insert data
            return InsertNodeAndAllChildren(rootNode, null);
        }

        private Path InsertNodeAndAllChildren(TreeNode<SimpleNode> rootNode, Path? dbParentPath = null)
        {
            var rootPath = AddNode(rootNode.Data.Title, dbParentPath);
            foreach (var child in rootNode.Children)
            {
                InsertNodeAndAllChildren(child, rootPath);
            }
            return rootPath;
        }

        private Path AddNode(string nodeTitle, Path? parentPath, bool saveOnAdd = true)
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