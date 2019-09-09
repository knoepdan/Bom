using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Data;
using Bom.Web.Areas.Main.Models;
using Bom.Web.Lib.Infrastructure;

namespace Bom.Web.Areas.Main.Controllers
{
    [Area("Main")]
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
        public async Task<ActionResult<IEnumerable<NodeVm>>> GetRootNodes()
        {
            var result = await _context.GetPaths().GetRootElements()
                .Include(x => x.Node)
                .Select(x => new NodeVm(x))
                .ToListAsync();
            return result;
        }


        [HttpGet("nodes")]
        public async Task<ActionResult<IEnumerable<NodeVm>>> GetNodes(TreeFilterInput filter)
        {
            var paths = _context.GetPaths();
            var basePath = await paths.FirstOrDefaultAsync(x => x.PathId == filter.BasePathId);
            if (basePath == null)
            {
                return NotFound();
            }

            // get search results
            var searchResult = new List<NodeVm>();
            if (filter.ParentDepth > 0)
            {
                var parents = paths.GetParents(basePath, filter.ParentDepth).Include(x => x.Node);
                searchResult.AddRange(parents.Select(x => new NodeVm(x)));
            }
            if (filter.ChildDepth > 0)
            {
                var children = paths.GetChildren(basePath, filter.ChildDepth).Include(x => x.Node);
                searchResult.AddRange(children.Select(x => new NodeVm(x)));
            }
            return searchResult;
        }



        // GET: api/Paths/5
        [HttpGet("{pathId}")]
        [HttpGet("nodeByPath")]
        public async Task<ActionResult<NodeVm>> GetNodeByPathId(int pathId)
        {
            var paths = _context.GetPaths().Include(x => x.Node);
            var path = await paths.FirstOrDefaultAsync(x => x.PathId == pathId);
            if (path != null)
            {
                return new NodeVm(path);
            }
            return NoContent();
        }

        [HttpGet("{nodeId}")]
        [HttpGet("nodeById")]
        public async Task<ActionResult<NodeVm>> GetNodeByNodeId(int nodeId)
        {
            var paths = _context.GetPaths().Include(x => x.Node);
            var path = await paths.FirstOrDefaultAsync(x => x.NodeId == nodeId);
            if (path != null)
            {
                return new NodeVm(path);
            }
            return NoContent(); // rather than NotFound(); (status codes only for true errors)
        }
    }
}