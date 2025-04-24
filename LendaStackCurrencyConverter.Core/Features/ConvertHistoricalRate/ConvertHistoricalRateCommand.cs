using LendastackCurrencyConverter.Core.Dto.Request;
using LendastackCurrencyConverter.Core.Dto.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Features.ConvertHistoricalRate
{
    public class ConvertHistoricalRateCommand : IRequest<BaseResponse<ExchangeRateResponseDto>>
    {
        public HistoricalConvertRequest HistoricalConvertRequest { get; set; }
    }
}
