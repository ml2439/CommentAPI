using CommentAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Services
{
    interface ICommentInfoRepository
    {
        IEnumerable<Comment> GetComments();
        Comment GetComment(int commentId, bool includeSubComments);
        IEnumerable<SubComment> GetSubComments(int commentId);
        SubComment GetSubCommentForComment(int commentId, int subCommentId);

    }
}
