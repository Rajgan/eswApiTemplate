using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Eshopworld.Tests.Core;
using FluentAssertions;
using Newtonsoft.Json;
using WebApi.Template.DataAccess;
using WebApi.Template.Model.DTO;
using Xunit;

namespace WebApi.Template.Tests.Intergration
{
    public class ValuesIntergrationTest : TestFixture
    {
        private const string Controller = "Values";
        private const string ApiVersion = "api/v1/";

        [Fact, IsIntegrationReadOnly]
        public async Task Test_Get()
        {
            var requestUri = new Uri($"{_apiBaseAddress}{ApiVersion}{Controller}");

            List<string> result = new List<string>();
            using (var response = await new HttpClient().GetAsync(requestUri))
            {
                response.EnsureSuccessStatusCode();
                var json = response.Content.ReadAsStringAsync().Result;
                
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<string>>(json);

                }
                result.Count.Should().BeGreaterThan(0);
            }
            
        }


        [Fact, IsIntegration]
        public async Task Test_Post()
        {
            var requestUri = new Uri($"{_apiBaseAddress}{ApiVersion}{Controller}");

            var request = new SampleRequest()
            {
                ClientNo = 1,
                Prefix = "Test",
                ClientCode = "Test Code"
            };

            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                "application/json");

            var response = await Client.PostAsync(requestUri, contentPost);
            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<SampleResponse>(json);

            result.Should().NotBeNull();
            //Add More Assert
            
        }
    }
}
