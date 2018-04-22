using System.Net;
using Eshopworld.Core;

namespace WebApi.Template.Model.TelemetryEvents
{
    public class TemplateApiErrorEvent : BbTelemetryEvent
    {
        public string ErrorJson { get; set; }
        public string ErrorType { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}
