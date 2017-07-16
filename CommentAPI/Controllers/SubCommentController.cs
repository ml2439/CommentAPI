using AutoMapper;
using CommentAPI.Entities;
using CommentAPI.Models;
using CommentAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Controllers
{
    [Route("api/comment")]
    public class SubCommentController : Controller
    {
        // Constructor injection
        private ILogger<SubCommentController> _logger;
        private IMailService _mailService;
        private ICommentInfoRepository _commentInfoRepository;

        public SubCommentController(ILogger<SubCommentController> logger,
            IMailService mailService,
            ICommentInfoRepository commentInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _commentInfoRepository = commentInfoRepository;
        }

        [HttpGet("{commentId}/subcomment")]
        public IActionResult GetSubComments(int commentId)
        {
            try
            {
                if (!_commentInfoRepository.CommentExists(commentId))
                {
                    _logger.LogInformation($"Comment with id {commentId} wasn't found when accessing subcomments.");
                    return NotFound();
                }

                var subCommentsForComment = _commentInfoRepository.GetSubCommentsForComment(commentId);

                var subCommentsForCommentResults = Mapper.Map<IEnumerable<SubCommentDto>>(subCommentsForComment);

                return Ok(subCommentsForCommentResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting subcomments of comment with id {commentId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{commentId}/subcomment/{subCommentId}", Name = "GetSubComment")]
        public IActionResult GetSubComment(int commentId, int subCommentId)
        {
            if (!_commentInfoRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var subComment = _commentInfoRepository.GetSubCommentForComment(commentId, subCommentId);
            if (subComment == null)
            {
                return NotFound();
            }

            var subCommentResult = Mapper.Map<SubCommentDto>(subComment);

            return Ok(subCommentResult);
        }

        [HttpPost("{commentId}/subcomment")]
        public IActionResult CreateSubComment(int commentId,
            [FromBody] SubCommentForCreationDto subComment)
        {
            // check user input (request body)
            if (subComment == null)
            {
                return BadRequest();
            }

            // check input validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_commentInfoRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var finalSubComment = Mapper.Map<SubComment>(subComment);

            _commentInfoRepository.AddSubComment(commentId, finalSubComment);

            if (!_commentInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdSubCommentToReturn = Mapper.Map<SubCommentDto>(finalSubComment);

            return CreatedAtRoute("GetSubComment",
                new { commentId = commentId, subCommentId = finalSubComment.Id },
                createdSubCommentToReturn);
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

            if (!_commentInfoRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var subCommentEntity = _commentInfoRepository.GetSubCommentForComment(commentId, subCommentId);
            if (subCommentEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(subComment, subCommentEntity);

            if (!_commentInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch("{commentId}/subcomment/{subCommentId}")]
        public IActionResult PartiallyUpdateSubComment(int commentId, int subCommentId,
            [FromBody] JsonPatchDocument<SubCommentForUpdateDto> patchDoc)
        {
            // check user input (request body)
            // PATCH request body must be an array of object(s)
            // Example: 
            /*
            [
	            {
		            "op": "replace",
		            "path": "/content",
		            "value":"a new content"

                },
	            {
		            "op": "replace",
		            "path": "/content",
		            "value":"another new content"
	            },
	            {
		            "op": "replace",
		            "path": "/nonexistcontent",
		            "value":"this should not be a valid patch"
	            }
            ]
            */
            if (patchDoc == null)
            {
                return BadRequest();
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

            var subCommentToPatch = new SubCommentForUpdateDto()
            {
                Content = subCommentFromStore.Content
            };

            // ModelState helps check input validation
            patchDoc.ApplyTo(subCommentToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate the patchdoc, make sure content value can't be null
            TryValidateModel(subCommentToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            subCommentFromStore.Content = subCommentToPatch.Content;

            return NoContent();
        }

        [HttpDelete("{commentId}/subcomment/{subCommentId}")]
        public IActionResult DeleteSubComment(int commentId, int subCommentId)
        {
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

            comment.SubComments.Remove(subCommentFromStore);

            _mailService.Send("SubComment deleted",
                $"SubComment of id {subCommentFromStore.Id} is deleted.");

            return NoContent();
        }
    }
}
