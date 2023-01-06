using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("OrderHeader")]
    public partial class OrderHeader
    {
        public OrderHeader()
        {
            OrderBatches = new HashSet<OrderBatch>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("isApproved")]
        public bool IsApproved { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }
        [Column("CustomerID")]
        [StringLength(50)]
        public string CustomerId { get; set; }
        [Required]
        [Column("EmployeeID")]
        [StringLength(50)]
        public string EmployeeId { get; set; }
        [Required]
        [Column("SiteID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal ShippingFee { get; set; }
        public int UsedPoint { get; set; }
        public int PayType { get; set; }
        [Column("OrderType_ID")]
        public int OrderTypeId { get; set; }
        [StringLength(50)]
        public string VoucherCode { get; set; }
        [StringLength(500)]
        public string Note { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("OrderHeaders")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(InternalUser.OrderHeaders))]
        public virtual InternalUser Employee { get; set; }
        [ForeignKey(nameof(OrderStatus))]
        [InverseProperty("OrderHeaders")]
        public virtual OrderStatus OrderStatusNavigation { get; set; }
        [ForeignKey(nameof(OrderTypeId))]
        [InverseProperty("OrderHeaders")]
        public virtual OrderType OrderType { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.OrderHeaders))]
        public virtual SiteInformation Site { get; set; }
        [ForeignKey(nameof(VoucherCode))]
        [InverseProperty(nameof(Voucher.OrderHeaders))]
        public virtual Voucher VoucherCodeNavigation { get; set; }
        [InverseProperty(nameof(OrderBatch.Order))]
        public virtual ICollection<OrderBatch> OrderBatches { get; set; }
    }
}
