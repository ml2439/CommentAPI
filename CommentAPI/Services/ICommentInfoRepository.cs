using CommentAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Services
{
    public interface ICommentInfoRepository
    {
        bool CommentExists(int commentId);
        IEnumerable<Comment> GetComments();
        Comment GetComment(int commentId, bool includeSubComments);
        IEnumerable<SubComment> GetSubCommentsForComment(int commentId);
        SubComment GetSubCommentForComment(int commentId, int subCommentId);
        void AddSubComment(int commentId, SubComment subComment);
        void DeleteSubComment(SubComment subComment);
        bool Save();
    }
}
