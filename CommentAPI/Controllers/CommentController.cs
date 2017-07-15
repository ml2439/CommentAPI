using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommentAPI.Controllers
{
    [Route("api/comment")]
    public class CommentController : Controller
    {
        public JsonResult GetComments()
        {
            return new JsonResult(new List<object>()
            {
                new { id=1, Content="this is a comment." },
                new { id=2, Content="this is another comment." }
            });
        }
    }
}
