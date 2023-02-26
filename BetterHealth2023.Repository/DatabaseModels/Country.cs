using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels
{
    [Table("Country")]
    public partial class Country
    {
        public Country()
        {
            Manufacturers = new HashSet<Manufacturer>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(2)]
        public string Iso { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        [StringLength(3)]
        public string Iso3 { get; set; }
        public int? NumCode { get; set; }
        public int PhoneCode { get; set; }

        [InverseProperty(nameof(Manufacturer.Country))]
        public virtual ICollection<Manufacturer> Manufacturers { get; set; }
    }
}
