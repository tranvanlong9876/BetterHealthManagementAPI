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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
