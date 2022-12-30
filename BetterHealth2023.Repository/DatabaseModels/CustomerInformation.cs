using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Keyless]
    [Table("Customer_Information")]
    public partial class CustomerInformation
    {
        [Required]
        [Column("Customer_ID")]
        [StringLength(50)]
        public string CustomerId { get; set; }
        public int? Gender { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDay { get; set; }
        public int? StoreRegisterId { get; set; }
        [Column("imageURL")]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
    }
}
