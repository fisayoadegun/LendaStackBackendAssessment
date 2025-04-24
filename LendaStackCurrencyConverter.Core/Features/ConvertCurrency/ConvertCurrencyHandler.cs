using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendastackCurrencyConverter.Infrastructure.Interface;
using LendastackCurrencyConverter.Core.Exceptions;
using AutoMapper;
using LendastackCurrencyConverter.Core.Dto.Response;
using Microsoft.Extensions.Logging;

namespace LendastackCurrencyConverter.Core.Features.ConvertCurrency
{
    public class ConvertCurrencyHandler : IRequestHandler<ConvertCurrencyCommand, BaseResponse<ExchangeRateResponseDto>>
    {    
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConvertCurrencyHandler> _logger;
        public ConvertCurrencyHandler(IExchangeRateRepository exchangeRateRepository, IMapper mapper, ILogger<ConvertCurrencyHandler> logger)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ExchangeRateResponseDto>> Handle(ConvertCurrencyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<ExchangeRateResponseDto>();
            try
            {
                var rate = await _exchangeRateRepository.GetRealTimeExchangeRate(request.ConvertRequest.BaseCurrency, request.ConvertRequest.TargetCurrency);
                if (rate == null)
                    throw new BadRequestException("No record found");
                if(rate.Rate == 0)
                    throw new BadRequestException("No record found");
                var data = _mapper.Map<ExchangeRateResponseDto>(rate);
                data.Amount = request.ConvertRequest.Amount;
                data.ConvertedAmount = request.ConvertRequest.Amount * data.Rate;
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
