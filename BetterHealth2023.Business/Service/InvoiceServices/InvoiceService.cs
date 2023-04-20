using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InvoiceModels;
using Firebase.Storage;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.InvoiceServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IOrderHeaderRepo _orderHeaderRepo;
        private readonly IOrderDetailRepo _orderDetailRepo;
        private readonly ISiteRepo _siteRepo;
        private readonly IDynamicAddressRepo _dynamicAddressRepo;

        public InvoiceService(IOrderHeaderRepo orderHeaderRepo, IOrderDetailRepo orderDetailRepo, ISiteRepo siteRepo, IDynamicAddressRepo dynamicAddressRepo)
        {
            _orderHeaderRepo = orderHeaderRepo;
            _orderDetailRepo = orderDetailRepo;
            _siteRepo = siteRepo;
            _dynamicAddressRepo = dynamicAddressRepo;
        }

        public async Task<IActionResult> PrintInvoicePdf(string orderId)
        {
            var orderDB = await _orderHeaderRepo.GetSpecificOrder(orderId);
            if (orderDB == null) return new BadRequestObjectResult("Mã đơn hàng không tồn tại");
            if (!orderDB.OrderTypeId.Equals(Commons.ORDER_TYPE_DIRECTLY)) return new BadRequestObjectResult("Chỉ được xuất hóa đơn đơn bán tại chỗ");

            var invoiceModel = new GenerateInvoiceModel();
            var siteDB = await _siteRepo.Get(orderDB.SiteId);
            var siteInfo = new SiteInformationModel()
            {
                SiteName = siteDB.SiteName,
                SiteAddress = await _dynamicAddressRepo.GetFullAddressFromAddressId(siteDB.AddressId)
            };
            invoiceModel.siteInformationModel = siteInfo;
            invoiceModel.CreatedDate = orderDB.CreatedDate;
            invoiceModel.ExportedDate = CustomDateTime.Now;
            invoiceModel.CustomerName = orderDB.orderContactInfo.Fullname;
            invoiceModel.CustomerEmail = orderDB.orderContactInfo.Email;
            invoiceModel.CustomerPhoneNo = orderDB.orderContactInfo.PhoneNumber;

            invoiceModel.OrderId = orderId;
            invoiceModel.ShippingPrice = orderDB.OrderTypeId.Equals(Commons.ORDER_TYPE_DELIVERY) ? orderDB.orderDelivery.ShippingFee : 0;
            invoiceModel.SubTotalPrice = orderDB.SubTotalPrice;
            invoiceModel.DiscountPrice = orderDB.DiscountPrice;
            invoiceModel.TotalPrice = orderDB.TotalPrice;

            var productInvoiceList = new List<ProductInvoiceModel>();
            var totalQuantity = 0;
            var productOrders = await _orderDetailRepo.GetViewSpecificOrderProducts(orderId);
            for (int i = 0; i < productOrders.Count; i++)
            {
                var product = productOrders[i];
                totalQuantity += product.Quantity;
                productInvoiceList.Add(new ProductInvoiceModel()
                {
                    ProductName = product.ProductName,
                    ProductQuantity = product.Quantity,
                    UnitName = product.UnitName,
                    ProductPrice = product.DiscountPrice,
                    TotalPrice = product.DiscountPrice * product.Quantity,
                    Note = string.IsNullOrEmpty(product.ProductNoteFromPharmacist) ? "Không có ghi chú" : product.ProductNoteFromPharmacist
                });
            }
            invoiceModel.productInvoiceModels = productInvoiceList;
            invoiceModel.TotalQuantity = totalQuantity;

            var document = await GenerateInvoiceService.GeneratePDFInvoice(invoiceModel);

            //return new FileContentResult(document, "application/pdf");

            var storage = StorageClient.Create();

            var bucketName = "better-health-3e75a.appspot.com";

            var objectName = "Invoice_" + invoiceModel.OrderId + "_" + CustomDateTime.Now.Ticks.ToString() + ".pdf";
            // Upload the PDF to Google Cloud Storage
            using (var memoryStream = new MemoryStream(document))
            {
                var contentType = "application/pdf";
                // Upload the file to the bucket with the given object name and content type
                await storage.UploadObjectAsync(bucketName, objectName, contentType, memoryStream);
            }

            var url = await new FirebaseStorage(bucketName).Child(objectName).GetDownloadUrlAsync();

            return new OkObjectResult(url);
        }
    }
}
