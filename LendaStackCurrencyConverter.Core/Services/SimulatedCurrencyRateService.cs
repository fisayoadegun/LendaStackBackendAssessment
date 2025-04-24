using LendastackCurrencyConverter.Core.Dto;
using LendastackCurrencyConverter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Services
{
    public class SimulatedCurrencyRateService : ICurrencyRateService
    {
        public async Task<RealTimeRateResponse> GetRealTimeRatesAsync(string baseCurrency)
        {
            await Task.Delay(300); //Simulating Network delay

            return new RealTimeRateResponse
            {
                Base = baseCurrency,
                Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Rates = new Dictionary<string, decimal>
                {
                    { "GBP", 0.80M },
                    { "EUR", 0.92M },
                    { "JPY", 155.00M }
                }
            };
        }


        public async Task<HistoricalRateResponse> GetHistoricalRatesAsync(
            string baseCurrency,
            string targetCurrency,
            DateTime from,
            DateTime to)
        {
            await Task.Delay(300);  //Simulating Network delay

            var rates = new Dictionary<DateTime, decimal>
            {
                { new DateTime(2025, 04, 01), 0.79M },
                { new DateTime(2025, 04, 02), 0.80M },
                { new DateTime(2025, 04, 03), 0.81M },
                { new DateTime(2025, 04, 04), 0.80M },
                { new DateTime(2025, 04, 05), 0.795M }
            };

            return new HistoricalRateResponse
            {
                Base = baseCurrency,
                Target = targetCurrency,
                Rates = rates
                    .Where(r => r.Key >= from && r.Key <= to)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };
        }

    }

}
