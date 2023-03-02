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
            OrderContactInfos = new HashSet<OrderContactInfo>();
            OrderDetails = new HashSet<OrderDetail>();
            OrderExecutions = new HashSet<OrderExecution>();
            OrderPickUps = new HashSet<OrderPickUp>();
            OrderShipments = new HashSet<OrderShipment>();
            OrderVouchers = new HashSet<OrderVoucher>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Column("PharmacistID")]
        [StringLength(50)]
        public string PharmacistId { get; set; }
        [Column("OrderTypeID")]
        public int OrderTypeId { get; set; }
        [Column("SiteID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }
        public double SubTotalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double TotalPrice { get; set; }
        public int UsedPoint { get; set; }
        public int PayType { get; set; }
        public bool IsPaid { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column("isApproved")]
        public bool? IsApproved { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }

        [ForeignKey(nameof(OrderStatus))]
        [InverseProperty("OrderHeaders")]
        public virtual OrderStatus OrderStatusNavigation { get; set; }
        [ForeignKey(nameof(OrderTypeId))]
        [InverseProperty("OrderHeaders")]
        public virtual OrderType OrderType { get; set; }
        [ForeignKey(nameof(PharmacistId))]
        [InverseProperty(nameof(InternalUser.OrderHeaders))]
        public virtual InternalUser Pharmacist { get; set; }
        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.OrderHeaders))]
        public virtual SiteInformation Site { get; set; }
        [InverseProperty(nameof(OrderBatch.Order))]
        public virtual ICollection<OrderBatch> OrderBatches { get; set; }
        [InverseProperty(nameof(OrderContactInfo.Order))]
        public virtual ICollection<OrderContactInfo> OrderContactInfos { get; set; }
        [InverseProperty(nameof(OrderDetail.Order))]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty(nameof(OrderExecution.Order))]
        public virtual ICollection<OrderExecution> OrderExecutions { get; set; }
        [InverseProperty(nameof(OrderPickUp.Order))]
        public virtual ICollection<OrderPickUp> OrderPickUps { get; set; }
        [InverseProperty(nameof(OrderShipment.Order))]
        public virtual ICollection<OrderShipment> OrderShipments { get; set; }
        [InverseProperty(nameof(OrderVoucher.Order))]
        public virtual ICollection<OrderVoucher> OrderVouchers { get; set; }
    }
}
