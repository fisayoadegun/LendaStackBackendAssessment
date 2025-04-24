using LendastackCurrencyConverter.Core.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendastackCurrencyConverter.Infrastructure;

namespace LendastackCurrencyConverter.Core.Features.ConvertCurrency
{
    public class ConvertCurrencyHandler : IRequestHandler<ConvertCurrencyCommand, BaseResponse<ExchangeRateResponseDto>>
    {        
        public ConvertCurrencyHandler()
        {
            
        }

        public Task<BaseResponse<ExchangeRateResponseDto>> Handle(ConvertCurrencyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
