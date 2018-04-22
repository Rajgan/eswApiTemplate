using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Eshopworld.Core;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.Template.Model;
using WebApi.Template.Model.Common;
using WebApi.Template.Model.Exceptions;
using WebApi.Template.Model.TelemetryEvents;

namespace WebApi.Template.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBigBrother _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, IBigBrother bigBrother)
        {
            _next = next;
            _logger = bigBrother ?? throw new ArgumentNullException(nameof(bigBrother), $"{nameof(IBigBrother)} isn't registred as a service."); 
            _logger?.DeveloperMode();
        }

        public virtual async Task Invoke(HttpContext context)
        {
            try
            {
                //Call the next delegate/ middleware in the pipeline
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        internal virtual async Task HandleExceptionAsync<T>(HttpContext context, T exception) where T : Exception
        {
            // Continuously unwrap AggregateException
            if (exception is AggregateException)
            {
                var aex = exception as AggregateException;
                aex.Flatten().Handle((ex) =>
                {
                    _logger.Publish(ex.ToBbEvent());
                    return true;
                });
            }
            else
            {
                _logger.Publish(exception.ToBbEvent());
            }
            
            var message = $"Oops! Sorry! Something went wrong." +
                          "Please contact eshop@support.com so we can try to fix it.";

            var errorExpection = new InternalServerException(exception)
            {
                ErrorTypeCode = ErrorTypeCode.InternalError,
                HttpStatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<Error>()
                {
                    new Error(){ErrorCode = "5001",ErrorKey = "ApiInternalServerError",ErrorMessage = message},
                }
            };

            await SendResponse(context, errorExpection);

        }

        internal virtual async Task HandleValidationExceptionAsync<T>(HttpContext context, T exception) where T : BaseException
        {
            var errEvent = new TemplateApiValidationErrorEvent()
            {
                StatusCode = exception.HttpStatusCode,
                ErrorJson = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented),
                ErrorType = exception.ErrorTypeCode.ToString()
            };

            _logger.Publish(errEvent);

            await SendResponse(context, exception);

        }

        internal  virtual async Task SendResponse<T>(HttpContext context, T exception) where T : BaseException
        {
            var errorResp = new ApiErrorResponse()
            {
                ErrorType = exception.ErrorTypeCode,
                ErrorIdentifier = GetTelemetryOperationId(context),
                ErrorDetails = exception.Errors,

            };
            var jsonSerlizationSetting = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var result = JsonConvert.SerializeObject(errorResp, jsonSerlizationSetting);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.HttpStatusCode;
            await context.Response.WriteAsync(result);
        }

        /// <summary>
        /// Get Operation Id from HttpContext Object
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal virtual string GetTelemetryOperationId(HttpContext context)
        {
            return context.Features.Get<RequestTelemetry>().Context.Operation.Id;
        }
    }
}
