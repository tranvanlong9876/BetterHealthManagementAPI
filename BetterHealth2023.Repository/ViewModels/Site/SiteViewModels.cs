using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class SiteViewModels
    {
        
      
        public string SiteName { get; set; }
        
        public string Description { get; set; }
     
        public string ContactInfo { get; set; }

        public string AddressID { get; set; }

    }
}
