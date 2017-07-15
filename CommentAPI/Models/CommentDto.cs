using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int NumberOfSubComments
        {
            get
            {
                return SubComments.Count;
            }
        }
        public ICollection<SubCommentDto> SubComments { get; set; } = new List<SubCommentDto>();
    }
}
