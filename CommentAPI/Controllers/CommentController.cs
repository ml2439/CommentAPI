using AutoMapper;
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

            var results = Mapper.Map<IEnumerable<CommentWithoutSubDto>>(commentEntities);

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
                var commentResult = Mapper.Map<CommentDto>(comment);
                return Ok(commentResult);
            }

            var commentWithoutSubResult = Mapper.Map<CommentWithoutSubDto>(comment);
            return Ok(commentWithoutSubResult);
        }
    }
}
