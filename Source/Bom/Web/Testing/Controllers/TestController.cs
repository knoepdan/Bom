using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Common;
using Bom.Web.Nodes.Models;
using Bom.Web.Common.Infrastructure;
using Bom.Core.Testing;
using System.Web;
using Bom.Core.Utils;
using Ch.Knomes.Struct;
using Ch.Knomes.Struct.Testing;
using Bom.Web.Common.Controllers;

namespace Bom.Web.Testing.Controllers
{
   // [Area("Test")]
    [Route("test")]
    [ApiController]
    public class TestController : BomBaseController
    {
        private readonly ModelContext _context;

        public TestController(ModelContext context)
        {
            _context = context;
        }

        [HttpPost("clearDatabase")]
        public IActionResult ClearDatabase()
        {
            var preparer = new TestDataPreparer(_context);
            preparer.CleanTestDatabase(); // info: will throw if db is not marked as test db
            return this.Ok();
        }

        [HttpPost("clearAndFillDatabase")]
        public IActionResult ClearAndFillDatabase(string? typeOfData = null)
        {
            TreeNode<SimpleNode> rootNode;
            typeOfData = typeOfData?.ToUpperInvariant();
            switch (typeOfData)
            {
                case "ANIMALS":
                    rootNode = TestDataFactory.CreateSampleAnimalNodes();
                    break;
                default:
                    rootNode = TestDataFactory.CreateSampleNodes(5,4);
                    break;
            }

            // fill database
            var preparer = new TestDataPreparer(_context);
            preparer.CreateTestData(rootNode); // info: will throw if db is not marked as test db

            return this.Ok();
        }

    }
}