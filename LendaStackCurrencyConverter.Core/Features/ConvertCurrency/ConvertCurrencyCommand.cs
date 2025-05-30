﻿using LendastackCurrencyConverter.Core.Dto.Request;
using LendastackCurrencyConverter.Core.Dto.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Features.ConvertCurrency
{
    public class ConvertCurrencyCommand : IRequest<BaseResponse<ExchangeRateResponseDto>>
    {
        public ConvertRequest ConvertRequest { get; set; }
    }
}
