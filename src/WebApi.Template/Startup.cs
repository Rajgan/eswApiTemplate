using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Eshopworld.Core;
using Eshopworld.DevOps;
using Eshopworld.Web;
using Eshopworld.Telemetry;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Template.DataAccess;
using WebApi.Template.Infrastructure;
using WebApi.Template.Infrastructure.Middleware;
using WebApi.Template.Validations;

namespace WebApi.Template
{
    /// <summary>
    /// Startup class for ASP.NET runtime
    /// </summary>
    public class Startup
    {
        internal readonly AppSettings AppSettings;
        private readonly IBigBrother _bb;
        private readonly IConfigurationRoot _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env">hosting environment</param>
        public Startup(IHostingEnvironment env)
        {
            _configuration = EswDevOpsSdk.BuildConfiguration(env.ContentRootPath, env.EnvironmentName);
            AppSettings = _configuration.GetSection("App").Get<AppSettings>();
            _bb = new BigBrother(AppSettings.Telemetry.InstrumentationKey, AppSettings.Telemetry.InternalKey);
        }

        /// <summary>
        /// configure services to be used by the asp.net runtime
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>service provider instance (Autofac provider)</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {                
                services.AddApplicationInsightsTelemetry(AppSettings.Telemetry.InstrumentationKey);
                ConfigureDatabase(services);
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        cpb => cpb
                               .AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials());
                });

                services.AddMvc(options =>
                {
                    var policy = ScopePolicy.Create(AppSettings.ServiceConfigurationOptions.RequiredScopes.ToArray());
                    options.Filters.Add(new AuthorizeFilter(policy));
                });

                services.AddApiVersioning(
                    o =>
                    {
                        o.ReportApiVersions = true;
                        o.AssumeDefaultVersionWhenUnspecified = true;
                        o.DefaultApiVersion = new ApiVersion(1, 0);
                    });

                services.AddSwaggerGen(c =>
                {
                    c.IncludeXmlComments("WebApi.Template.xml");
                    c.DescribeAllEnumsAsStrings();
                    c.SwaggerDoc("v1", new Info { Version = "1.0.0", Title = "WebApi.Template" });
                    c.CustomSchemaIds(x => x.FullName);
                    c.AddSecurityDefinition("Bearer",
                        new ApiKeyScheme
                        {
                            In = "header",
                            Description = "Please insert JWT with Bearer into field",
                            Name = "Authorization",
                            Type = "apiKey"
                        });
                });

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddIdentityServerAuthentication(x =>
                {
                    x.ApiName = AppSettings.ServiceConfigurationOptions.ApiName;
                    x.SupportedTokens = SupportedTokens.Jwt;
                    x.RequireHttpsMetadata = AppSettings.ServiceConfigurationOptions.IsHttps;
                    x.Authority = AppSettings.ServiceConfigurationOptions.Authority;
                    x.RequireHttpsMetadata = AppSettings.ServiceConfigurationOptions.IsHttps;
                    //TODO: this requires Eshopworld.Beatles.Security to be refactored
                    //x.AddJwtBearerEventsTelemetry(bb); 
                });
                
                services.AddMvc().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<NameAddressesValidator>());

                var builder = new ContainerBuilder();
                builder.Populate(services);
                builder.RegisterInstance(_bb).As<IBigBrother>().SingleInstance();
                builder.RegisterInstance(AppSettings).AsSelf().SingleInstance();

                // add additional services or modules into container here

                var container = builder.Build();
                return new AutofacServiceProvider(container);
            }
            catch (Exception e)
            {
                _bb.Publish(e.ToBbEvent());
                throw;
            }
        }

        /// <summary>
        /// configure asp.net pipeline
        /// </summary>
        /// <param name="app">application builder</param>
        /// <param name="env">environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();
               loggerFactory.AddConsole(_configuration.GetSection("Logging"));
               loggerFactory.AddDebug();
            }
           else
           {
               app.UseBigBrotherExceptionHandler();
           }
           app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors("CorsPolicy");
            app.UseSwagger(o => o.RouteTemplate="swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("v1/swagger.json", "WebApi.Template");
                o.RoutePrefix = "swagger";              
            });
           app.UseAuthentication();
           app.UseMvcWithDefaultRoute();

        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<TemplateDbContext>(options =>
                options.UseSqlServer(AppSettings.ConnectionString.PlatformConnection,
                    retOptions => retOptions.EnableRetryOnFailure()));
        }
    }
}
