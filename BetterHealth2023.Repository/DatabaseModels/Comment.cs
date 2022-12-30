using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("Product_ID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        [Column("User_ID")]
        [StringLength(50)]
        public string UserId { get; set; }
        [Column("Comment_Role")]
        public int? CommentRole { get; set; }
        [Required]
        [StringLength(50)]
        public string CreateDate { get; set; }
        public int Leverage { get; set; }
        [Required]
        [Column("ParentID")]
        [StringLength(50)]
        public string ParentId { get; set; }
        [Column("Comment_Information_ID")]
        [StringLength(50)]
        public string CommentInformationId { get; set; }

        [ForeignKey(nameof(CommentInformationId))]
        [InverseProperty("Comments")]
        public virtual CommentInformation CommentInformation { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.Comments))]
        public virtual ProductDetail Product { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Customer.Comments))]
        public virtual Customer User { get; set; }
    }
}
