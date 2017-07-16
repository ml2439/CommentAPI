using CommentAPI.Entities;
using CommentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Extensions
{
    public static class CommentInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CommentInfoContext context)
        {
            if (context.Comments.Any())
            {
                return;
            }

            // init seed data
            var comments = new List<Comment>()
            {
                new Comment()
                {
                    Content = "New City",
                    SubComments = new List<SubComment>()
                    {
                        new SubComment()
                        {
                            Content = "first sub of 1"
                        },
                        new SubComment()
                        {
                            Content = "sec sub of 1"
                        }
                    }
                },
                new Comment()
                {
                    Content = "2 City",
                    SubComments = new List<SubComment>()
                    {
                        new SubComment()
                        {
                            Content = "first sub of 2"
                        }
                    }
                },
                new Comment()
                {
                    Content = "3 City",
                    SubComments = new List<SubComment>()
                    {
                        new SubComment()
                        {
                            Content = "first sub of 3"
                        },
                        new SubComment()
                        {
                            Content = "sec sub of 3"
                        }
                    }
                },
            };

            context.Comments.AddRange(comments);    // track
            context.SaveChanges();      // insert
        }
    }
}
