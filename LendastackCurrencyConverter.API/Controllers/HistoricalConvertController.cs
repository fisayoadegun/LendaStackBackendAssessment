using LendastackCurrencyConverter.Core.Dto.Request;
using LendastackCurrencyConverter.Core.Features.ConvertHistoricalRate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendastackCurrencyConverter.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/convert/historical")]

    public class HistoricalConvertController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HistoricalConvertController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("convert-historical-rate")]
        public async Task<IActionResult> Convert([FromBody] ConvertHistoricalRateCommand request)
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

