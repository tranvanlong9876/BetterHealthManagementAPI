using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Comment_Information")]
    public partial class CommentInformation
    {
        public CommentInformation()
        {
            Comments = new HashSet<Comment>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(50)]
        public string PhoneNo { get; set; }
        public bool Anonymous { get; set; }

        [InverseProperty(nameof(Comment.CommentInformation))]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
