using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Data;
using Bom.Core.Model;
using Bom.Core.DataAccess;
using Bom.Web.Areas.Main.Models;
using Bom.Web.Lib.Infrastructure;
using Bom.Core.Testing;
using System.Web;
using Bom.Core.Utils;

namespace Bom.Web.Areas.Testing.Controllers
{
    [Area("Test")]
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
        public IActionResult ClearAndFillDatabase(string typeOfData = null)
        {
            TreeNode<SimpleNode> rootNode;
            typeOfData = typeOfData?.ToLowerInvariant();
            switch (typeOfData)
            {
                case "animals":
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