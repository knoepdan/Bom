using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Data;
using Web.Lib.Infrastructure;

namespace Bom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BomBaseController
    {
        private readonly ModelContext _context;

        public ValuesController(ModelContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var firstPath = _context.GetPaths().FirstOrDefault();
            var firstNode = _context.GetNodes().FirstOrDefault();

            return new string[] { "value1", "value2", $"Path: {firstPath}" , $"firstNode: {firstNode}"};
        }

        [HttpGet("ex")]
        public ActionResult<string> Ex(int id)
        {
            throw new Utils.Error.AppException(Utils.Error.ErrorCode.NotFound, "blabla", null, "this is the usermessage");
           // return "value";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
