using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Template.Common.Extensions;
using WebApi.Template.DataAccess;
using WebApi.Template.Model;
using WebApi.Template.Model.Common;
using WebApi.Template.Model.DTO;
using WebApi.Template.Model.Exceptions;

namespace WebApi.Template.Controllers
{
    /// <summary>
    /// sample controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class ValuesController : Controller
    {
        private readonly TemplateDbContext _context;
        private readonly IValidator<SampleRequest> _validator;

        internal virtual string AiOperationId => HttpContext.Features.Get<RequestTelemetry>().Context.Operation.Id;

        public ValuesController(TemplateDbContext context, IValidator<SampleRequest> validator)
        {
            _context = context;
            _validator = validator;
        }

        /// <summary>
        /// GET implementation for default route
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<string>))]
        public IActionResult Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        /// <summary>
        /// Get with Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        public IActionResult Get(int id)
        {
            if (id == 0)
            {
                return BadRequest(new ApiErrorResponse()
                {
                    ErrorIdentifier = AiOperationId,
                    ErrorDetails= new Error() { ErrorCode = "1001", ErrorKey = "Id", ErrorMessage = "Invalid Id Passed" }.ToErrorList(),
                    ErrorType = ErrorTypeCode.BusinessError
                });
            }
            return Ok("value");
        }

        /// <summary>
        /// post
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SampleResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        public async Task<IActionResult> Post([FromBody] SampleRequest request)
        {
           

            //Sample Business Error
            if (request.ClientNo == 0)
            {
                return BadRequest(new ApiErrorResponse()
                {
                    ErrorIdentifier = AiOperationId,
                    ErrorDetails = new Error() { ErrorCode = "1001", ErrorKey = "Id", ErrorMessage = "Invalid Id Passed" }.ToErrorList(),
                    ErrorType = ErrorTypeCode.BusinessError
                });
                
            }

            // Sample Validation and return errors
            var requestValidation = await _validator.ValidateAsync(request, ruleSet: "ValidateForPost");

            if (!requestValidation.IsValid)
            {
                return BadRequest(new ApiErrorResponse()
                {
                    ErrorIdentifier = AiOperationId,
                    ErrorDetails = requestValidation.Errors.Select(x => new Error()
                    {
                        ErrorMessage = x.ErrorMessage,
                        ErrorKey = x.PropertyName,
                        ErrorCode = x.ErrorCode
                    }).ToList(),
                    ErrorType = ErrorTypeCode.ValidationError
                });
            }

            //Sample Error Handling with Sub methods
            (string result, Error error) = SampleSubMethod(request.ClientNo);
            if (error != null)
            {
                return BadRequest(new ApiErrorResponse()
                {
                    ErrorIdentifier = AiOperationId,
                    ErrorDetails = error.ToErrorList(),
                    ErrorType = ErrorTypeCode.BusinessError
                });
            }
            
            var response = new SampleResponse()
            {
                CarrierCode = "Test Code",
                CarrierName = "Test Name",
                Prefix = "Test Prefix"
            };
            return Ok(response);
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SampleResponse))]
       public async Task<IActionResult> Put(int id, [FromBody]SampleRequest request)
        {
            if (id == 0)
            {
                throw new Exception("Client 0 not allowed");
            }
            var response = new SampleResponse()
            {
                CarrierCode = "Test Code",
                CarrierName = "Test Name",
                Prefix = "Test Prefix"
            };
            return Ok(response);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        internal (string result, Error error) SampleSubMethod(int value)
        {
            switch (value)
            {
                case 1: return ("Sucess",null);
                case var cli when ((cli >= 2) && (cli <= 20)): return ("Sucess 2", null);
                default:
                    return (string.Empty,
                        new Error()
 {
                            ErrorKey = "Sample Method",
                            ErrorCode = "4003",
                            ErrorMessage = "Invalid Value Passed"
                        });
            }
        }
        
    }
}
