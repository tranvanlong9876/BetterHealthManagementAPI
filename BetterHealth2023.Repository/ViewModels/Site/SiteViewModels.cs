using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.DynamicAddressViewModel;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site
{
    public class SiteViewModels
    {
        
      
        public string SiteName { get; set; }
        
        public string Description { get; set; }
     
        public string ContactInfo { get; set; }

        public DynamicAddModel DynamicAddModel { get; set; }


    }
}
