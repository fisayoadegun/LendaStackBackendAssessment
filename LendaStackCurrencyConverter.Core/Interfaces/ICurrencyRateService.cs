using LendastackCurrencyConverter.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Interfaces
{
    public interface ICurrencyRateService
    {
        Task<RealTimeRateResponse> GetRealTimeRatesAsync(string baseCurrency);
        Task<HistoricalRateResponse> GetHistoricalRatesAsync(string baseCurrency, string targetCurrency, DateTime from, DateTime to);
    }
}
