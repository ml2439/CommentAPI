using CommentAPI.Models;
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
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.SubComments);
        }

        [HttpGet("{commentId}/subcomment/{subCommentId}", Name = "GetSubComment")]
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

        [HttpPost("{commentId}/subcomment")]
        public IActionResult CreateSubComment(int commentId, [FromBody] SubCommentForCreationDto subComment)
        {
            // check user input (request body)
            if (subComment == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check uri to make sure commentId exist (resource uri)
            var comment = CommentDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            // calculate id for new input (map SubCommentDto to SubCommentForCreationDto)
            // temp: mannual mapping. Will change to auto id generate later
            var maxSubCommentId = CommentDataStore.Current.Comments.SelectMany(
                c => c.SubComments).Max(sc => sc.Id);
            var finalSubComment = new SubCommentDto()
            {
                Id = ++maxSubCommentId,
                Content = subComment.Content
            };
            comment.SubComments.Add(finalSubComment);

            return CreatedAtRoute("GetSubComment",
                new { commentId = commentId, subCommentId = finalSubComment.Id },
                finalSubComment);
        }

        [HttpPut("{commentId}/subcomment/{subCommentId}")]
        public IActionResult UpdateSubComment(int commentId, int subCommentId, 
            [FromBody] SubCommentForUpdateDto subComment)
        {
            // check user input (request body)
            if (subComment == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check uri to make sure commentId exist (resource uri)
            var comment = CommentDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var subCommentFromStore = comment.SubComments.FirstOrDefault(sbfs => sbfs.Id == subCommentId);
            if (subCommentFromStore == null)
            {
                return NotFound();
            }

            subCommentFromStore.Content = subComment.Content;

            return NoContent();
        }

    }
}
