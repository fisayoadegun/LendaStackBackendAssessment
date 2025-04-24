using AutoMapper;
using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Exceptions;
using LendastackCurrencyConverter.Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Features.ConvertHistoricalRate
{
    public class ConvertHistoricalRateHandler : IRequestHandler<ConvertHistoricalRateCommand, BaseResponse<ExchangeRateResponseDto>>
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConvertHistoricalRateHandler> _logger;
        public ConvertHistoricalRateHandler(IExchangeRateRepository exchangeRateRepository, IMapper mapper, ILogger<ConvertHistoricalRateHandler> logger)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ExchangeRateResponseDto>> Handle(ConvertHistoricalRateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<ExchangeRateResponseDto>();

            try
            {
                var exchangeRate = await _exchangeRateRepository.GetHistoricalExchangeRate(request.HistoricalConvertRequest.BaseCurrency,
                                        request.HistoricalConvertRequest.TargetCurrency, request.HistoricalConvertRequest.Date.Date);
                if (exchangeRate == null)
                    throw new BadRequestException("No record found");
                if (exchangeRate.Rate == 0)
                    throw new BadRequestException("No record found");
                var data = _mapper.Map<ExchangeRateResponseDto>(exchangeRate);
                data.Amount = request.HistoricalConvertRequest.Amount;
                data.ConvertedAmount = request.HistoricalConvertRequest.Amount * data.Rate;
                response.Data = data;
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Success = false;
                _logger.LogError($"{ex.Message}");
                return response;
            }
        }
    }
}
