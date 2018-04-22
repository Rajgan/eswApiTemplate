using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Template.DataAccess;

namespace WebApi.Template.Tests.Intergration
{
    public class TestStartup : Startup
    {
        private readonly IHostingEnvironment _env;

        public TestStartup(IHostingEnvironment env) : base(env)
        {
            _env = env;
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            var databaseName = $"testDB_Platform_{Guid.NewGuid()}";

            services.AddDbContext<TemplateDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));

            //TODO Add your Seed Data
        }
    }
}
