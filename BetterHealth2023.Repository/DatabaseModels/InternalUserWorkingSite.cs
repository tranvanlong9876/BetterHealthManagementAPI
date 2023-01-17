using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("InternalUser_WorkingSite")]
    public partial class InternalUserWorkingSite
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [Column("UserID")]
        [StringLength(50)]
        public string UserId { get; set; }
        [Required]
        [Column("SiteID")]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("isWorking")]
        public bool IsWorking { get; set; }

        [ForeignKey(nameof(SiteId))]
        [InverseProperty(nameof(SiteInformation.InternalUserWorkingSites))]
        public virtual SiteInformation Site { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(InternalUser.InternalUserWorkingSites))]
        public virtual InternalUser User { get; set; }
    }
}
