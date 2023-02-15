using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ManufacturerModels
{
    public class ViewManufacturerList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }

        public int ProductUsing { get; set; }
    }
}
