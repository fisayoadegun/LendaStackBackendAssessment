using LendastackCurrencyConverter.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Infrastructure.Interface
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> GetRealTimeExchangeRate(string baseCurrency, string targetCurrency);
        Task<ExchangeRate> AddExchangeRate(ExchangeRate exchangeRate);
        Task<List<ExchangeRate>> GetHistoricalRates(string baseCurrency, string targetCurrency, DateTime startDate, DateTime endDate);
        Task<ExchangeRate> GetHistoricalExchangeRate(string baseCurrency, string targetCurrency, DateTime date);

        Task<bool> ExistsAsync(string baseCurrency, string targetCurrency, DateTime date);
    }
}
