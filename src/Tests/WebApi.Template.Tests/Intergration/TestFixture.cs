using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using WebApi.Template.Infrastructure;

namespace WebApi.Template.Tests.Intergration
{
    public class TestFixture : IDisposable
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected readonly Uri _apiBaseAddress;
        internal readonly AppSettings AppSettings;
        private readonly IConfigurationRoot _configuration;
        public TestFixture()
        {
            if (Server == null)
            {
                _configuration = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .AddJsonFile($"appsettings.Test.json", optional: true)
                             .Build();
                Server = new TestServer(WebHost.CreateDefaultBuilder()
                                               .UseStartup<TestStartup>()
                                               .UseEnvironment("TEST")
                                               .UseContentRoot(Directory.GetCurrentDirectory())
                                               .UseConfiguration(_configuration));
            }

            Client = Server.CreateClient();
            AppSettings = _configuration.GetSection("App").Get<AppSettings>();

            _apiBaseAddress = new Uri(AppSettings.ApiSetting.ApiBaseAddress);
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
