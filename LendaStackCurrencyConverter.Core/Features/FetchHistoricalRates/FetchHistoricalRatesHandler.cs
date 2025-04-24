using AutoMapper;
using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Exceptions;
using LendastackCurrencyConverter.Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LendastackCurrencyConverter.Core.Features.FetchHistoricalRates
{
    public class FetchHistoricalRatesHandler : IRequestHandler<FetchHistoricalRatesCommand, BaseResponse<List<ExchangeRateResponseDto>>>
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FetchHistoricalRatesHandler> _logger;
        public FetchHistoricalRatesHandler(IExchangeRateRepository exchangeRateRepository, IMapper mapper, ILogger<FetchHistoricalRatesHandler> logger)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ExchangeRateResponseDto>>> Handle(FetchHistoricalRatesCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<ExchangeRateResponseDto>>();
            try
            {
                var exchangeRates = await _exchangeRateRepository.GetHistoricalRates(request.HistoricalRateRequest.BaseCurrency, request.HistoricalRateRequest.TargetCurrency,
                                                request.HistoricalRateRequest.StartDate, request.HistoricalRateRequest.EndDate);
                if (!exchangeRates.Any())
                    throw new BadRequestException("No Record Found");

                var data = _mapper.Map<List<ExchangeRateResponseDto>>(exchangeRates);

                response.Success = true;
                response.Data = data;
                _logger.LogInformation("Historical Rates Fetched successfully");
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
