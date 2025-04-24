using AutoMapper;
using FluentValidation;
using LendastackCurrencyConverter.Core.Dto.Request;
using LendastackCurrencyConverter.Core.Features.ConvertCurrency;
using LendastackCurrencyConverter.Core.Features.FetchHistoricalRates;
using LendastackCurrencyConverter.Core.Features.FetchUpdateHistorycalRates;
using LendastackCurrencyConverter.Core.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendastackCurrencyConverter.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/historical")]
    [ApiKeyAuthorize]
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
            var validator = new FetchUpdateHistoricalRateValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }
            var response = await _mediator.Send(request);
            if (response.Success)
                return Ok(response);

            else if (response.Error == "No record found")
                return NotFound(response);

            else
                return BadRequest(response);

        }
        
        [HttpGet("get-historical-rates")]
        public async Task<IActionResult> GetHistoricalRates(
                            [FromQuery] string baseCurrency,
                            [FromQuery] string targetCurrency,
                            [FromQuery] DateTime startDate,
                            [FromQuery] DateTime endDate)
        {
            var request = new HistoricalRateRequest
            {
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency,
                StartDate = startDate.Date,
                EndDate = endDate.Date
            };

            var command = new FetchHistoricalRatesCommand
            {
                HistoricalRateRequest = request
            };


            var validator = new FetchHistoricalRatesValidator();
            var result = validator.Validate(command);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            var response = await _mediator.Send(command);
            if (response.Success)
                return Ok(response);

            else if (response.Error == "No record found")
                return NotFound(response);

            else
                return BadRequest(response);

        }
    }
}
