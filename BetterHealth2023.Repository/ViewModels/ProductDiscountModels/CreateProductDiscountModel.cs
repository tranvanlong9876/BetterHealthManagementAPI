using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductDiscountModels
{
    [AtLeastOneProperty("DiscountPercent", "DiscountMoney", ErrorMessage = "Phải có tối thiểu 1 trong 2 field DiscountPercent và DiscountMoney có dữ liệu")]
    public class CreateProductDiscountModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Reason { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountMoney { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public List<CreateProductDiscountList> Products { get; set; }
    }

    public class CreateProductDiscountList
    {
        [Required]
        public string ProductId { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; set; }

        public AtLeastOnePropertyAttribute(params string[] propertyList)
        {
            this.PropertyList = propertyList;
        }

        //See http://stackoverflow.com/a/1365669
        public override object TypeId
        {
            get
            {
                return this;
            }
        }
        public override bool IsValid(object value)
        {

            //  Need to use reflection to get properties of "value"...
            PropertyInfo propertyInfo;
            foreach (string propertyName in PropertyList)
            {
                propertyInfo = value.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
