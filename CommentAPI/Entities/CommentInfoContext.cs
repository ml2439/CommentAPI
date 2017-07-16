using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Entities
{
    public class CommentInfoContext : DbContext
    {
        public CommentInfoContext(DbContextOptions<CommentInfoContext> options)
            : base(options)
        {
            // Using code-first approch. This line ensures database is created if not exist yet.
            // But this only registers the context on container, doesn't create an instance of it.
            // So use a dummy controller to create the database.
            Database.Migrate();
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
    }
}
