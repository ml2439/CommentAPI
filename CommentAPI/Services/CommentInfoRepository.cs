using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentAPI.Services
{
    public class CommentInfoRepository : ICommentInfoRepository
    {
        private CommentInfoContext _context;
        public CommentInfoRepository(CommentInfoContext context)
        {
            _context = context;
        }

        public bool CommentExists(int commentId)
        {
            return _context.Comments.Any(c => c.Id == commentId);
        }

        public Comment GetComment(int commentId, bool includeSubComments)
        {
            if (includeSubComments)
            {
                return _context.Comments.Include(c => c.SubComments)
                    .Where(c => c.Id == commentId).FirstOrDefault();
            }

            return _context.Comments.Where(c => c.Id == commentId).FirstOrDefault();
        }

        public IEnumerable<Comment> GetComments()
        {
            return _context.Comments.OrderBy(c => c.Content).ToList();
        }

        public SubComment GetSubCommentForComment(int commentId, int subCommentId)
        {
            return _context.SubComments.Where(s => s.CommentId == commentId && s.Id == subCommentId).FirstOrDefault();
        }

        public IEnumerable<SubComment> GetSubCommentsForComment(int commentId)
        {
            return _context.SubComments.Where(s => s.CommentId == commentId).ToList();
        }
    }
}
