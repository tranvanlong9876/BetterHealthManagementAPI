using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.UpdateProductModels
{
    public class UpdateProductEntranceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string subCategoryId { get; set; }
        public string manufacturerId { get; set; }
        public bool isPrescription { get; set; }

        public List<UpdateProductDetailModel> productDetailModel { get; set; }

        public UpdateProductDescriptionModel descriptionModel { get; set; }
    }
}
