using CommentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI
{
    public class CommentDataStore
    {
        // Make sure work on the same data unless restart
        // auto property initialize syntax (new in C# 6)
        public static CommentDataStore Current { get; } = new CommentDataStore();   

        public List<CommentDto> Comments { get; set; }

        public CommentDataStore()
        {
            Comments = new List<CommentDto>()
            {
                new CommentDto()
                {
                    Id = 1,
                    Content = "New City"
                },
                new CommentDto()
                {
                    Id = 2,
                    Content = "comment b"
                },
                new CommentDto()
                {
                    Id = 3,
                    Content = "N comment c"
                }
            };
        }
    }
}
