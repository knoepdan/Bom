using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Model;
using Core.DataAccess;
using Web.Areas.Main.Models;
using Web.Lib.Infrastructure;
using Core.Testing;
using System.Web;

namespace Web.Areas.Testing.Controllers
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
            var dataFactory = new TestDataFactory();
            TreeNode<MemoryNode> rootNode;
            typeOfData = typeOfData?.ToLowerInvariant();
            switch (typeOfData)
            {
                case "animals":
                    rootNode = dataFactory.CreateSampleAnimalNodes();
                    break;
                default:
                    rootNode = dataFactory.CreateSampleNodes();
                    break;
            }

            // fill database
            var preparer = new TestDataPreparer(_context);
            preparer.CreateTestData(rootNode); // info: will throw if db is not marked as test db

            return this.Ok();
        }

    }
}