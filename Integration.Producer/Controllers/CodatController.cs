using Integration.Models;
using Integration.Producer.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Integration.Producer.Controllers
{
    [Route("integrations/accounting/[controller]")]
    [ApiController]
    public class CodatController : ControllerBase
    {
        readonly ICodatService _codatService;
        public CodatController(ICodatService codatService)
        {
            _codatService = codatService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Ok("Codat Service is up and running");
        }

        /// <summary>
        /// Push data to the Kafka queue
        /// </summary>
        /// <param name="codatRequest"></param>
        /// <param name="codatBody"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("events")]
        public IActionResult PostEvents([FromQuery] CodatQueryParameters codatRequest, [FromBody] CodatBody codatBody)
        {
            var codatEvent = new CodatEvent()
            {
                CodatRequestParameters = codatRequest,
                CodatBody = codatBody
            };

            var statusCode = _codatService.PerformAction(codatEvent) ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
            return new StatusCodeResult(statusCode);
        }
    }
}
