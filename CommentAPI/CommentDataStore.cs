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
                    Content = "New City",
                    SubComments = new List<SubCommentDto>()
                    {
                        new SubCommentDto()
                        {
                            Id = 1,
                            Content = "first sub of 1"
                        },
                        new SubCommentDto()
                        {
                            Id = 2,
                            Content = "sec sub of 1"
                        }
                    }
                },
                new CommentDto()
                {
                    Id = 2,
                    Content = "2 City",
                    SubComments = new List<SubCommentDto>()
                    {
                        new SubCommentDto()
                        {
                            Id = 3,
                            Content = "first sub of 2"
                        }
                    }
                },
                new CommentDto()
                {
                    Id = 3,
                    Content = "3 City",
                    SubComments = new List<SubCommentDto>()
                    {
                        new SubCommentDto()
                        {
                            Id = 4,
                            Content = "first sub of 3"
                        },
                        new SubCommentDto()
                        {
                            Id = 5,
                            Content = "sec sub of 3"
                        }
                    }
                },
            };
        }
    }
}
