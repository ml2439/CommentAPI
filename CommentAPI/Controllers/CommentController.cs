using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CommentAPI.Controllers
{
    [Route("api/comment")]
    public class CommentController : Controller
    {
        [HttpGet]
        public JsonResult GetComments()
        {
            return new JsonResult(CommentDataStore.Current.Comments);
        }

        [HttpGet("{id}")]
        public JsonResult GetComment(int id)
        {
            return new JsonResult(CommentDataStore.Current.Comments.FirstOrDefault(c => c.Id == id));
        }
    }
}
