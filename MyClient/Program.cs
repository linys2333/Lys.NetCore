using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyClient
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // 从元数据中发现端口
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("lys", "123", "api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // 调用api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            client.DefaultRequestHeaders.Add("userId", Guid.Empty.ToString());

            var response = await client.PostAsync("http://localhost:5001/api/CallRecord", GetContent());
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JObject.Parse(content));
            }

            Console.WriteLine();
        }

        private static HttpContent GetContent()
        {
            var content = new MultipartFormDataContent();

            var fileName = "1.amr";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = fileName,
                Name = "file"
            };

            var mobileContent = new StringContent("18688888888");
            mobileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "Mobile"
            };

            var durationContent = new StringContent("10");
            durationContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "Duration"
            };

            content.Add(fileContent);
            content.Add(mobileContent);
            content.Add(durationContent);

            return content;
        }
    }
}
