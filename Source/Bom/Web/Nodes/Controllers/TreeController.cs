using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Common;
using Bom.Core.Nodes;
using Bom.Core.Nodes.DbModels;
using Bom.Web.Nodes.Models;
using Bom.Web.Common.Infrastructure;

namespace Bom.Web.Nodes.Controllers
{
  //  [Area("Main")] -> not needed for api controllers
    [Route("main")]
    [ApiController]
    public class TreeController : BomBaseController
    {
        private readonly ModelContext _context;

        public TreeController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Paths
        [HttpGet("root")]
        public async Task<ListAnswer<NodeVm>> GetRootNodes()
        {
            var result = await _context.Paths.AllRootPaths()
                .Include(x => x.Node)
                .Select(x => new NodeVm(x)).ToListAsync();
            return Answer.CreateList(result);
        }


        [HttpGet("nodes")]
        public async Task<ActionResult<ListAnswer<NodeVm>>> GetNodes(TreeFilterInput filter)
        {
            var paths = _context.Paths;
            if(paths == null)
            {
                return NotFound();
            }
            var basePath = await paths.FirstOrDefaultAsync(x => x.PathId == filter.BasePathId);
            if (basePath == null)
            {
                return NotFound();
            }

            // get search results
            var searchResult = new List<NodeVm>();
            if (filter.ParentDepth > 0)
            {
                var parents = paths.Ancestors(basePath, filter.ParentDepth).Include(x => x.Node);
                searchResult.AddRange(parents.Select(x => new NodeVm(x)));
            }
            if (filter.ChildDepth > 0)
            {
                var children = paths.Descendants(basePath, filter.ChildDepth).Include(x => x.Node);
                searchResult.AddRange(children.Select(x => new NodeVm(x)));
            }
            return Answer.CreateList(searchResult);
        }



        // GET: api/Paths/5
   //     [HttpGet("{pathId}")]
        [HttpGet("nodeByPath/{pathId}")]
        public async Task<Answer<NodeVm>> GetNodeByPathId(int pathId)
        {
            var paths = _context.Paths.Include(x => x.Node);
            var path = await paths.FirstOrDefaultAsync(x => x.PathId == pathId);
            if (path != null)
            {
                return Answer.Create(new NodeVm(path));
            }
            // TODO -> switch to a default result object that is always returned 
            //return this.Ok(); // NoContent(); would return 200
            return new Answer<NodeVm>(); ;
        }

      //  [HttpGet("{nodeId}")]
        [HttpGet("nodeById/{nodeId}")]
        public async Task<Answer<NodeVm>> GetNodeByNodeId(int nodeId)
        {
            var paths = _context.Paths.Include(x => x.Node);
            var path = await paths.FirstOrDefaultAsync(x => x.NodeId == nodeId);
            if (path != null)
            {
                return Answer.Create(new NodeVm(path));
            }
            // TODO -> switch to a default result object that is always returned 
            return new Answer<NodeVm>(); // NoContent(); // rather than NotFound(); (status codes only for true errors)
        }
    }
}