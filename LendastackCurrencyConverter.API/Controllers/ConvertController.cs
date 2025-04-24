using LendastackCurrencyConverter.Core.Dto;
using LendastackCurrencyConverter.Core.Features.ConvertCurrency;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LendastackCurrencyConverter.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/convert")]
    public class ConvertController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ConvertController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("convert-real-time-rate")]
        public async Task<IActionResult> Convert([FromBody] ConvertCurrencyCommand request)
        {            
            var response = await _mediator.Send(request);
            if (response.Success)
                return Ok(response);

            else if (response.Error == "No record found")
                return NotFound(response);

            else
                return BadRequest(response);
           
        }
    }
}
