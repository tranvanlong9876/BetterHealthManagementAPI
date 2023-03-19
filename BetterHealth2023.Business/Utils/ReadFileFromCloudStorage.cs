using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public static class ReadFileFromCloudStorage
    {
        public static async Task<string> ReadFileFromGoogleCloudStorage(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    return await reader.ReadToEndAsync();
                }
            }

        }
    }
}
