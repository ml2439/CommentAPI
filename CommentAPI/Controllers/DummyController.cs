using CommentAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Controllers
{
    public class DummyController : Controller
    {
        private CommentInfoContext _ctx;
        public DummyController(CommentInfoContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
