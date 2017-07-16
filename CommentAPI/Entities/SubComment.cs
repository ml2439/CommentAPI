using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommentAPI.Entities
{
    public class SubComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Content { get; set; }

        // By convention, a relation will be created when there's a navigation property discovered on a type.
        // A property is considered as a navigation property if the type it points to cannot be mapped as a scalar type by the database provider.
        // Example: prop Comment of type Comment is considered as a navigation property. A relation is created. Comment's Id is the foreign key by default.
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        public int CommentId { get; set; }
    }
}
