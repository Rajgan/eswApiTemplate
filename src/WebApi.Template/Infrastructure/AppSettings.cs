using System.Collections.Generic;
using Eshopworld.DevOps;

namespace WebApi.Template.Infrastructure
{
    public class AppSettings
    {
        public ConnectionString ConnectionString { get; set; }
        public TelemetrySettings Telemetry { get; set; }
        public ServiceConfigurationOption ServiceConfigurationOptions { get; set; }
        public ApiSetting ApiSetting { get; set; }
    }

    public class ApiSetting
    {
        public string ApiBaseAddress { get; set; }
    }

    public class ConnectionString
    {
        public string PlatformConnection { get; set; }
    }

    public class ServiceConfigurationOption
    {
            /// <summary>
            /// The scopes to assert on the Api
            /// </summary>
            public List<string> RequiredScopes { get; set; }

            public List<string> Origins { get; set; }

            /// <summary>
            /// The Authority base address
            /// </summary>
            public string Authority { get; set; }

            /// <summary>
            /// The Api name
            /// </summary>
            public string ApiName { get; set; }

            /// <summary>
            /// The Api secret
            /// </summary>
            public string ApiSecret { get; set; }

            /// <summary>
            /// Indicates if the service should require https
            /// </summary>
            public bool IsHttps => !string.IsNullOrWhiteSpace(Authority) && Authority.StartsWith("https");
        
    }
}
