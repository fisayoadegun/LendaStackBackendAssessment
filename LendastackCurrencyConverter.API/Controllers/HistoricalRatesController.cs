using AutoMapper;
using LendastackCurrencyConverter.Core.Features.ConvertCurrency;
using LendastackCurrencyConverter.Core.Features.FetchHistoricalRates;
using LendastackCurrencyConverter.Core.Features.FetchUpdateHistorycalRates;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendastackCurrencyConverter.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/historical")]
    public class HistoricalRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HistoricalRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("fetch-and-update-historical-rates")]
        public async Task<IActionResult> FetchUpdateHistoricalRates([FromBody] FetchUpdateHistoricalRateCommand request)
        {
            var response = await _mediator.Send(request);
            if (response.Success)
                return Ok(response);

            else if (response.Error == "No record found")
                return NotFound(response);

            else
                return BadRequest(response);

        }

        [HttpGet("get-historical-rates")]
        public async Task<IActionResult> GetHistoricalRates([FromBody] FetchHistoricalRatesCommand request)
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
