using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilityController : ControllerBase
    {
        [HttpGet("GetCurrentTime")]
        [AllowAnonymous]
        public IActionResult GetCurrentTime()
        {
            return Ok(CustomDateTime.Now);
        } 
        [HttpPost("UploadFile")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadImagesAsync(IFormFile file)
        {
            var imageUrl = String.Empty;
            if (file == null) return BadRequest("Chưa chọn ảnh");
            try
            {

                var fileName = Guid.NewGuid().ToString() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.imgbb.com/1/upload?key=0428fd7476dca636c92c4b4c3a96a87d");

                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(file.OpenReadStream())
                        {
                            Headers =
                        {
                            ContentLength = file.Length,
                            ContentType = new MediaTypeHeaderValue(file.ContentType)
                        }
                        }, "image", fileName);

                        var response = await client.PostAsync("https://api.imgbb.com/1/upload?key=0428fd7476dca636c92c4b4c3a96a87d", content);
                        response.EnsureSuccessStatusCode();
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        dynamic responseObject = JObject.Parse(responseContent);
                        imageUrl = responseObject.data.url;
                    }
                }
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("UploadMultipleFile")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadMultipleImages(List<IFormFile> fileList)
        {
            if (!fileList.Any()) return BadRequest("Chưa có ảnh");
            var imageList = new List<string>();
            try
            {
                foreach (var file in fileList)
                {
                    var imageUrl = String.Empty;
                    if (file == null) return BadRequest("Chưa chọn ảnh");
                    var fileName = Guid.NewGuid().ToString() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://api.imgbb.com/1/upload?key=0428fd7476dca636c92c4b4c3a96a87d");

                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StreamContent(file.OpenReadStream())
                            {
                                Headers =
                        {
                            ContentLength = file.Length,
                            ContentType = new MediaTypeHeaderValue(file.ContentType)
                        }
                            }, "image", fileName);

                            var response = await client.PostAsync("https://api.imgbb.com/1/upload?key=0428fd7476dca636c92c4b4c3a96a87d", content);
                            response.EnsureSuccessStatusCode();
                            var responseContent = response.Content.ReadAsStringAsync().Result;
                            dynamic responseObject = JObject.Parse(responseContent);
                            imageUrl = responseObject.data.url;
                            imageList.Add(imageUrl);
                        }
                    }

                }
                return Ok(imageList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
