using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Common;
using Bom.Core.Nodes.DbModels;
using Bom.Web.Lib.Infrastructure;

namespace Bom.Web.Controllers
{


    // JUST TESTING
    [Route("api/[controller]")]
    [ApiController]
    public class PathsController : ControllerBase
    {
        private readonly ModelContext _context;

        public PathsController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Paths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Path>>> GetPaths()
        {
            return await _context.Paths.ToListAsync();
        }

        // GET: api/Paths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Path>> GetPath(int id)
        {
            var path = await _context.FindPathAsync(id);

            if (path == null)
            {
                return NotFound();
            }

            return path;
        }

        // PUT: api/Paths/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPath(int id, Path path)
        {
            if (id != path.PathId)
            {
                return BadRequest();
            }

            _context.Entry(path).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PathExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //// POST: api/Paths
        //[HttpPost]
        //public async Task<ActionResult<Path>> PostPath(Path path)
        //{
        //    _context.Paths.Add(path);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPath", new { id = path.PathId }, path);
        //}

        //// DELETE: api/Paths/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Path>> DeletePath(int id)
        //{
        //    var path = await _context.Paths.FindAsync(id);
        //    if (path == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Paths.Remove(path);
        //    await _context.SaveChangesAsync();

        //    return path;
        //}

        private bool PathExists(int id)
        {
            if(this._context.Paths == null)
            {
                return false;
            }
            return this._context.Paths.Any(e => e.PathId == id);
        }
    }
}
