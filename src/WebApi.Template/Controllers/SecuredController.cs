using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Template.DataAccess;

namespace WebApi.Template.Controllers
{
    /// <summary>
    /// sample controller
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class SecuredController : Controller
    {
        private readonly TemplateDbContext _context;

        public SecuredController(TemplateDbContext context)
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
        [AllowAnonymous]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// post
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
