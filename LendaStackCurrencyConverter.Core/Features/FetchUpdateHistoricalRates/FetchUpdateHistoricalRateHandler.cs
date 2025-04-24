using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Exceptions;
using LendastackCurrencyConverter.Core.Interfaces;
using LendastackCurrencyConverter.Infrastructure.Interface;
using LendastackCurrencyConverter.Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Features.FetchUpdateHistorycalRates
{
    public class FetchUpdateHistoricalRateHandler : IRequestHandler<FetchUpdateHistoricalRateCommand, BaseResponse<string>>
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ICurrencyRateService _currencyRateService;
        private readonly ILogger<FetchUpdateHistoricalRateHandler> _logger;
        public FetchUpdateHistoricalRateHandler(IExchangeRateRepository exchangeRateRepository, ICurrencyRateService currencyRateService, ILogger<FetchUpdateHistoricalRateHandler> logger)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRateService = currencyRateService;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(FetchUpdateHistoricalRateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();
            try
            {
                var rates = await _currencyRateService.GetHistoricalRatesAsync(request.HistoricalRateRequest.BaseCurrency, request.HistoricalRateRequest.TargetCurrency,
                                                request.HistoricalRateRequest.StartDate.Date, request.HistoricalRateRequest.EndDate.Date);
                if (!rates.Rates.Any())
                    throw new BadRequestException("No record found");

                if (rates.Base == "No Record" || rates.Target == "No Record")
                    throw new BadRequestException("No record found");

                int count = 0;
                foreach(var rate in rates.Rates)
                {
                    var exists = await _exchangeRateRepository.ExistsAsync(
                            request.HistoricalRateRequest.BaseCurrency,
                            request.HistoricalRateRequest.TargetCurrency,
                            rate.Key
                        );
                    if (exists)
                        continue;

                    var dataToAdd = new ExchangeRate
                    {
                        BaseCurrency = request.HistoricalRateRequest.BaseCurrency,
                        TargetCurrency = request.HistoricalRateRequest.TargetCurrency,
                        Rate = rate.Value,
                        Date = rate.Key,
                        IsRealTime = false
                    }; 
                    await _exchangeRateRepository.AddExchangeRate(dataToAdd);
                    count++;
                }

                if(count == 0)
                {
                    response.Success = true;
                    response.Message = "All Historical rates have been saved for the currency pair already";
                    _logger.LogInformation($"{response.Message}");
                    return response;
                }
                response.Success = true;
                response.Count = count;
                response.Message = "Historical rates saved successfully";
                _logger.LogInformation($"{response.Message}");
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                _logger.LogError($"{ex.Message}");
                return response;
            }            
        }
    }
}
