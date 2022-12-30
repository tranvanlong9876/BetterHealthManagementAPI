using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Customer_Card")]
    public partial class CustomerCard
    {
        [Key]
        [Column("Card_No")]
        [StringLength(100)]
        public string CardNo { get; set; }
        [Required]
        [Column("Customer_ID")]
        [StringLength(50)]
        public string CustomerId { get; set; }
        public int Point { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("CustomerCards")]
        public virtual Customer Customer { get; set; }
    }
}
