using BarcodeLib;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.InvoiceModels;
using IronPdf;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public class GenerateInvoiceService
    {
        private static string BETTERHEALTH_LOGO_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/BetterHealth-Logo.png?alt=media&token=d0832ae8-eabb-4642-afdf-ebdf93b587ae";
        private static string CHECKOUTCARTTEMPLATE_PDF_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/CheckOutCartTemplatePdf.html?alt=media&token=bc82835a-ab94-4fe7-b1c7-d2daca644bc1";
        private static string CHECKOUTCARTPARTIAL_PDF_URL = "https://firebasestorage.googleapis.com/v0/b/better-health-3e75a.appspot.com/o/CheckOutCartPartialPdf.html?alt=media&token=230d44e3-6d3f-437c-9369-2c67a3f8d897";

        public static string body;
        public async static Task<byte[]> GeneratePDFInvoice(GenerateInvoiceModel invoiceModel)
        {
            string cartHtml = string.Empty;
            string dynamicRow = await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTPARTIAL_PDF_URL);//File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\InvoiceTemplate\CheckOutCartPartialPdf.html");//await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTPARTIAL_URL);
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
                eachRow = eachRow.Replace("{{productPriceTotal}}", EmailService.ConvertToVietNamCurrency(productModel.TotalPrice));

                cartHtml = cartHtml + eachRow;
            }
            body = await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTTEMPLATE_PDF_URL); //File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\InvoiceTemplate\CheckOutCartTemplatePdf.html");//await ReadFileFromCloudStorage.ReadFileFromGoogleCloudStorage(CHECKOUTCARTTEMPLATE_URL);
            body = body.Replace("{{orderId}}", invoiceModel.OrderId);
            body = body.Replace("{{createdDate}}", "(" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", invoiceModel.CreatedDate) + ")");
            body = body.Replace("{{exportedDate}}", String.Format("{0:dd/MM/yyyy HH:mm:ss}", invoiceModel.ExportedDate));
            body = body.Replace("{{siteName}}", invoiceModel.siteInformationModel.SiteName);
            body = body.Replace("{{siteAddress}}", invoiceModel.siteInformationModel.SiteAddress);
            body = body.Replace("{{employeeName}}", invoiceModel.siteInformationModel.EmployeeName);
            body = body.Replace("{{logoUrl}}", BETTERHEALTH_LOGO_URL);
            body = body.Replace("{{cart}}", cartHtml);
            
            if (!string.IsNullOrEmpty(invoiceModel.CustomerEmail))
            {
                string emailHtml = "<span style=\"font-size: 15px;\">Email: <a href=\"mailto:{{email}}\" target=\"_blank\">{{email}}</a><br /></span>";
                emailHtml = emailHtml.Replace("{{email}}", invoiceModel.CustomerEmail);
                body = body.Replace("{{email}}", emailHtml);
            }
            else
            {
                body = body.Replace("{{email}}", "");
            }

            if (!string.IsNullOrEmpty(invoiceModel.CustomerPhoneNo))
            {
                string phoneHtml = "<span style=\"font-size:15px;\">Sđt: {{phone}}</span>";
                phoneHtml = phoneHtml.Replace("{{phone}}", invoiceModel.CustomerPhoneNo);
                body = body.Replace("{{phone}}", phoneHtml);
            }
            else
            {
                body = body.Replace("{{phone}}", "");
            }
            body = body.Replace("{{name}}", string.IsNullOrEmpty(invoiceModel.CustomerName) ? "Khách vãng lai" : invoiceModel.CustomerName);
            body = body.Replace("{{phone}}", invoiceModel.CustomerPhoneNo);
            body = body.Replace("{{subTotalPrice}}", EmailService.ConvertToVietNamCurrency(invoiceModel.SubTotalPrice));
            body = body.Replace("{{discount_total}}", EmailService.ConvertToVietNamCurrency(invoiceModel.DiscountPrice));
            body = body.Replace("{{total_order}}", EmailService.ConvertToVietNamCurrency(invoiceModel.TotalPrice));
            body = body.Replace("{{hotline_phone}}", "19006969");

            var barcode = new Barcode();
            var image = barcode.Encode(TYPE.CODE128, invoiceModel.OrderId, Color.Black, Color.Transparent, 300, 100);

            var memoryStream = new MemoryStream();

            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            var barcodeBase64 = Convert.ToBase64String(memoryStream.ToArray());

            memoryStream.Close();

            body = body.Replace("{{barcode-img}}", $"data:image/png;base64,{barcodeBase64}");

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

        public async static Task<byte[]> GeneratePDF(string body, string orderId)
        {
            //var converter = new HtmlToPdf();
            //converter.Options.PdfPageSize = PdfPageSize.A6;
            //converter.Options.MaxPageLoadTime = 240;
            //PdfDocument document = converter.ConvertHtmlString(RemoveVietnameseTone(body));
            //return Task.FromResult(document.Save());

            var renderer = new ChromePdfRenderer();
            //renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A5;
            var pdf = await renderer.RenderHtmlAsPdfAsync(body);
            return pdf.BinaryData;
        }
    }
}
