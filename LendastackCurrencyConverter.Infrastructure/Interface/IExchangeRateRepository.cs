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
    }
}
