using CommentAPI.Models;
using CommentAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CommentAPI.Controllers
{
    [Route("api/comment")]
    public class CommentController : Controller
    {
        private ICommentInfoRepository _commentInfoRepository;
        public CommentController(ICommentInfoRepository commentInfoRepository)
        {
            _commentInfoRepository = commentInfoRepository;
        }

        [HttpGet]
        public IActionResult GetComments()
        {
            //return Ok(CommentDataStore.Current.Comments);
            var commentEntities = _commentInfoRepository.GetComments();

            var results = new List<CommentWithoutSubDto>();

            foreach (var ce in commentEntities)
            {
                results.Add(new CommentWithoutSubDto()
                {
                    Id = ce.Id,
                    Content = ce.Content
                });
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(int id, bool includeSubComments = false)
        {
            var comment = _commentInfoRepository.GetComment(id, includeSubComments);
            if (comment == null)
            {
                return NotFound();
            }

            if (includeSubComments)
            {
                var commentResult = new CommentDto()
                {
                    Id = comment.Id,
                    Content = comment.Content
                };

                foreach(var sc in comment.SubComments)
                {
                    commentResult.SubComments.Add(
                        new SubCommentDto()
                        {
                            Id = sc.Id,
                            Content = sc.Content
                        });
                }

                return Ok(commentResult);
            }

            var commentWithoutSubResult = new CommentWithoutSubDto()
            {
                Id = comment.Id,
                Content = comment.Content
            };

            return Ok(commentWithoutSubResult);

            //var comment = CommentDataStore.Current.Comments?.FirstOrDefault(c => c.Id == id);
            //if (comment == null)
            //{
            //    return NotFound();
            //}

            //return Ok(comment);
        }
    }
}
