using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("OrderID")]
        [StringLength(50)]
        public string OrderId { get; set; }
        [Required]
        [Column("ProductID")]
        [StringLength(50)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double PriceTotal { get; set; }
        [StringLength(500)]
        public string Note { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderDetails))]
        public virtual OrderHeader Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(ProductDetail.OrderDetails))]
        public virtual ProductDetail Product { get; set; }
    }
}
