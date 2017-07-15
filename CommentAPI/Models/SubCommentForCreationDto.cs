using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Models
{
    public class SubCommentForCreationDto
    {
        [Required(ErrorMessage = "You should provide content.")]
        [MaxLength(200)]
        public string Content { get; set; }
    }
}
