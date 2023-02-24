using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Order_Shipment")]
    public partial class OrderShipment
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderId { get; set; }
        [StringLength(50)]
        public string StartAddressId { get; set; }
        [Required]
        [StringLength(50)]
        public string DestinationAddressId { get; set; }
        public double ShippingFee { get; set; }

        [ForeignKey(nameof(DestinationAddressId))]
        [InverseProperty(nameof(DynamicAddress.OrderShipmentDestinationAddresses))]
        public virtual DynamicAddress DestinationAddress { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(OrderHeader.OrderShipments))]
        public virtual OrderHeader Order { get; set; }
        [ForeignKey(nameof(StartAddressId))]
        [InverseProperty(nameof(DynamicAddress.OrderShipmentStartAddresses))]
        public virtual DynamicAddress StartAddress { get; set; }
    }
}
