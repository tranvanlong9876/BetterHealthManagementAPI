using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.CreateProductModels
{
    public class CreateProductModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string SubCategoryId { get; set; }

        [StringLength(50)]
        public string ManufacturerId { get; set; }

        [Required]
        public bool IsPrescription { get; set; }

        [Required]
        public bool IsBatches { get; set; }

        /// <summary>
        /// Đối tượng sử dụng: null hoặc "" (mọi đối tượng), 1 (trẻ em), 2 (người lớn), 3 (người cao tuổi), 4 (phụ nữ cho con bú).
        /// </summary>
        [Range(1, 4, ErrorMessage = "Dữ liệu không hợp lệ, vui lòng nằm trong khoảng 1 -> 4 hoặc rỗng.")]
        public string UserUsageTarget { get; set; }

        [Required]
        public List<CreateProductDetailModel> productDetailModel { get; set; }

        [Required]
        public CreateProductDescriptionModel descriptionModel { get; set; }

        public List<CreateProductImage> imageModel { get; set; }

    }

    public class CreateProductImage
    {
        [Required]
        [RegularExpression(@"\b(?:https?://|www\.)\S+\b", ErrorMessage = "Invalid image URL")]
        public string imageURL { get; set; }
        public bool? IsFirstImage { get; set; }

    }
}
