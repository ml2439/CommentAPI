using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CommentAPI.Controllers
{
    [Route("api/comment")]
    public class CommentController : Controller
    {
        [HttpGet]
        public IActionResult GetComments()
        {
            return Ok(CommentDataStore.Current.Comments);
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(int id)
        {
            var comment = CommentDataStore.Current.Comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
    }
}
