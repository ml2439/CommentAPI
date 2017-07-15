using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Controllers
{
    [Route("api/comment")]
    public class SubCommentController : Controller
    {
        [HttpGet("{commentId}/subcomment")]
        public IActionResult GetSubComments(int commentId)
        {
            var comment = CommentDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.SubComments);
        }

        [HttpGet("{commentId}/subcomment/{subCommentId}")]
        public IActionResult GetSubComment(int commentId, int subCommentId)
        {
            var comment = CommentDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }
            var subComment = comment.SubComments.FirstOrDefault(sc => sc.Id == subCommentId);
            if (subComment == null)
            {
                return NotFound();
            }

            return Ok(subComment);
        }
    }
}
