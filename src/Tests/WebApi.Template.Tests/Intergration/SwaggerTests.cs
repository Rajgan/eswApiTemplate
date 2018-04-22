using System;
using System.Linq;
using System.Threading.Tasks;
using Eshopworld.Tests.Core;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace WebApi.Template.Tests.Intergration
{
    public class SwaggerTests : TestFixture
    {
        private const string TargetUrl = "/swagger/v1.0/swagger.json";

        [Fact, IsIntegration]
        public async Task Verify()
        {
            string swaggerResult;
            using (var client = new System.Net.WebClient())
            {
                Console.WriteLine("Url SwaggerTests " + _apiBaseAddress + TargetUrl);
                swaggerResult = await client.DownloadStringTaskAsync(_apiBaseAddress + TargetUrl);
            }
            
            swaggerResult.Should().NotBeNull();

            var obj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(swaggerResult);
            var paths = obj["paths"];
            Assert.True(paths.Children().Count() > 1);
        }
    }
}
