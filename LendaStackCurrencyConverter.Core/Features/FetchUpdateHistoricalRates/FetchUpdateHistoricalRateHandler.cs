using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Exceptions;
using LendastackCurrencyConverter.Core.Interfaces;
using LendastackCurrencyConverter.Infrastructure.Interface;
using LendastackCurrencyConverter.Infrastructure.Models;
using MediatR;
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
        public FetchUpdateHistoricalRateHandler(IExchangeRateRepository exchangeRateRepository, ICurrencyRateService currencyRateService)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRateService = currencyRateService;
        }

        public async Task<BaseResponse<string>> Handle(FetchUpdateHistoricalRateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();
            try
            {
                var rates = await _currencyRateService.GetHistoricalRatesAsync(request.HistoricalRateRequest.BaseCurrency, request.HistoricalRateRequest.TargetCurrency,
                                                request.HistoricalRateRequest.StartDate, request.HistoricalRateRequest.EndDate);
                if (!rates.Rates.Any())
                    throw new BadRequestException("No record Found");

                if (rates.Base == "No Record" || rates.Target == "No Record")
                    throw new BadRequestException("No record Found");
                foreach(var rate in rates.Rates)
                {
                    var dataToAdd = new ExchangeRate
                    {
                        BaseCurrency = request.HistoricalRateRequest.BaseCurrency,
                        TargetCurrency = request.HistoricalRateRequest.TargetCurrency,
                        Rate = rate.Value,
                        Date = rate.Key,
                        IsRealTime = false
                    }; 
                    await _exchangeRateRepository.AddExchangeRate(dataToAdd);
                }
                response.Success = true;
                response.Count = rates.Rates.Count;
                response.Message = "Historical rates saved successfully";
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                return response;
            }            
        }
    }
}
