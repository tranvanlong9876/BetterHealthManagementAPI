using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InvoiceModels;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public class GenerateInvoiceService
    {
        private static string BETTERHEALTH_LOGO_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/BetterHealth-Logo.png?alt=media&token=d0832ae8-eabb-4642-afdf-ebdf93b587ae";
        public static string body;
        public async static Task<byte[]> GeneratePDFInvoice(GenerateInvoiceModel invoiceModel)
        {
            string cartHtml = string.Empty;
            string dynamicRow = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\InvoiceTemplate\CheckOutCartPartialPdf.html");//await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTPARTIAL_URL);
            for (int i = 0; i < invoiceModel.productInvoiceModels.Count; i++)
            {
                var productModel = invoiceModel.productInvoiceModels[i];
                string eachRow = dynamicRow;
                //string eachRow = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\WebTemplate\CheckOutCartPartial.html");
                eachRow = eachRow.Replace("{{countNumber}}", (i + 1).ToString());
                eachRow = eachRow.Replace("{{productName}}", productModel.ProductName);
                eachRow = eachRow.Replace("{{unitName}}", productModel.UnitName);
                eachRow = eachRow.Replace("{{productPrice}}", EmailService.ConvertToVietNamCurrency(productModel.ProductPrice));
                eachRow = eachRow.Replace("{{productQuantity}}", productModel.ProductQuantity.ToString());
                eachRow = eachRow.Replace("{{productPriceTotal}}", EmailService.ConvertToVietNamCurrency(productModel.ProductPrice));

                cartHtml = cartHtml + eachRow;
            }
            body = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\InvoiceTemplate\CheckOutCartTemplatePdf.html");//await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTTEMPLATE_URL);
            body = body.Replace("{{orderId}}", invoiceModel.OrderId);
            body = body.Replace("{{createdDate}}", "(" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", invoiceModel.CreatedDate) + ")");
            body = body.Replace("{{exportedDate}}", String.Format("{0:dd/MM/yyyy HH:mm:ss}", invoiceModel.ExportedDate));
            body = body.Replace("{{siteName}}", invoiceModel.siteInformationModel.SiteName);
            body = body.Replace("{{siteAddress}}", invoiceModel.siteInformationModel.SiteAddress);
            body = body.Replace("{{logoUrl}}", BETTERHEALTH_LOGO_URL);
            body = body.Replace("{{cart}}", cartHtml);
            body = body.Replace("{{email}}", invoiceModel.CustomerEmail);
            body = body.Replace("{{name}}", string.IsNullOrEmpty(invoiceModel.CustomerName) ? "Khách vãng lai" : invoiceModel.CustomerName);
            body = body.Replace("{{phone}}", invoiceModel.CustomerPhoneNo);
            body = body.Replace("{{subTotalPrice}}", EmailService.ConvertToVietNamCurrency(invoiceModel.SubTotalPrice));
            body = body.Replace("{{discount_total}}", EmailService.ConvertToVietNamCurrency(invoiceModel.DiscountPrice));
            body = body.Replace("{{total_order}}", EmailService.ConvertToVietNamCurrency(invoiceModel.TotalPrice));
            body = body.Replace("{{hotline_phone}}", "19006969");

            return await GeneratePDF(body, invoiceModel.OrderId);
        }

        public static string RemoveVietnameseTone(string text)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string formD = text.Normalize(NormalizationForm.FormD);
            return regex.Replace(formD, String.Empty)
                        .Replace('\u0111', 'd')
                        .Replace('\u0110', 'D')
                        .Normalize(NormalizationForm.FormC);
        }

        public static async Task<byte[]> GeneratePDF(string body, string orderId)
        {
            var converter = new HtmlToPdf();
            PdfDocument document = converter.ConvertHtmlString(RemoveVietnameseTone(body));
            return document.Save();
        }
    }
}
