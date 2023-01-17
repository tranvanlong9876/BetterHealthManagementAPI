using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Customer")]
    public partial class Customer
    {
        public Customer()
        {
            Comments = new HashSet<Comment>();
            CustomerAddresses = new HashSet<CustomerAddress>();
            CustomerPoints = new HashSet<CustomerPoint>();
            OrderHeaders = new HashSet<OrderHeader>();
            PurchaseVoucherHistories = new HashSet<PurchaseVoucherHistory>();
            VoucherCustomerRestrictions = new HashSet<VoucherCustomerRestriction>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(30)]
        public string PhoneNo { get; set; }
        public int Status { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Password { get; set; }
        [StringLength(200)]
        public string PasswordSalt { get; set; }
        public int? Gender { get; set; }
        [Column("DOB", TypeName = "date")]
        public DateTime? Dob { get; set; }
        [Required]
        [Column("imageURL")]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(Comment.User))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(CustomerAddress.Customer))]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        [InverseProperty(nameof(CustomerPoint.Customer))]
        public virtual ICollection<CustomerPoint> CustomerPoints { get; set; }
        [InverseProperty(nameof(OrderHeader.Customer))]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        [InverseProperty(nameof(PurchaseVoucherHistory.Customer))]
        public virtual ICollection<PurchaseVoucherHistory> PurchaseVoucherHistories { get; set; }
        [InverseProperty(nameof(VoucherCustomerRestriction.Customer))]
        public virtual ICollection<VoucherCustomerRestriction> VoucherCustomerRestrictions { get; set; }
    }
}
