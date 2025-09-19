using LendastackCurrencyConverter.Core.Dto;
using LendastackCurrencyConverter.Core.Features.ConvertCurrency;
using LendastackCurrencyConverter.Core.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LendastackCurrencyConverter.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/convert")]
    [ApiKeyAuthorize]
    public class ConvertController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ConvertController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Converts a specified amount from one currency to another using real-time exchange rates.
        /// </summary>
        /// <remarks>
        /// **Description:**  
        /// This endpoint allows clients to convert a monetary amount from a base currency to a target currency 
        /// using the latest available real-time exchange rates.  
        /// 
        /// **Request Example:**  
        /// POST /api/convert/convertions 
        /// 
        /// ```json
        /// {
        ///   "convertRequest": {
        ///     "baseCurrency": "USD",
        ///     "targetCurrency": "EUR",
        ///     "amount": 100.50
        ///   }
        /// }
        /// ```
        /// 
        /// **Successful Response Example (200 OK):**  
        /// ```json
        /// {
        ///   "baseCurrency": "USD",
        ///   "targetCurrency": "EUR",
        ///   "rate": 0.92,
        ///   "date": "2025-09-19T10:15:30Z",
        ///   "isRealTime": true,
        ///   "amount": 100.50,
        ///   "convertedAmount": 92.46
        /// }
        /// ```
        /// 
        /// **Error Response Examples:**  
        /// - **400 Bad Request**: Invalid request parameters or validation errors.  
        /// ```json
        /// [ "BaseCurrency is required", "Amount must be greater than 0" ]
        /// ```
        /// 
        /// - **404 Not Found**: When no exchange rate record is found for the given currencies.  
        /// ```json
        /// {
        ///   "success": false,
        ///   "error": "No record found"
        /// }
        /// ```
        /// 
        /// </remarks>
        /// <param name="request">The conversion request containing base currency, target currency, and amount.</param>
        /// <response code="200">Returns the converted amount and exchange rate details.</response>
        /// <response code="400">If validation fails or request is invalid.</response>
        /// <response code="404">If no exchange rate is found for the given currency pair.</response>
        [HttpPost("conversions")]
        public async Task<IActionResult> Convert([FromBody] ConvertCurrencyCommand request)
        {
            var validator = new ConvertCurrencyValidator();
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
    }
}
