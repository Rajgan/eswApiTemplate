using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Template.DataAccess;
using WebApi.Template.Model;
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

        public ValuesController(TemplateDbContext context)
        {
            _context = context;
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
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// post
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<SampleResponse>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiErrorResponse))]
        public async Task<IActionResult> Post([FromBody]SampleRequest request)
        {
            if (request.ClientNo == 0)
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
        /// Put
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<SampleResponse>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiErrorResponse))]
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
    }
}
