using AutoMapper;
using LendastackCurrencyConverter.Core.Dto;
using LendastackCurrencyConverter.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ExchangeRate, ExchangeRateResponseDto>().ReverseMap();
        }

    }
}
