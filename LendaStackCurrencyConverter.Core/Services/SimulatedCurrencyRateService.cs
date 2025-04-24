using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Interfaces;
using Polly;
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
            return await ExecuteWithRetryAsync(() =>
            {
                return Task.FromResult( new RealTimeRateResponse
                {
                    Base = baseCurrency,
                    Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    Rates = new Dictionary<string, decimal>
                    {
                        { "GBP", 0.80M },
                        { "EUR", 0.92M },
                        { "JPY", 155.00M }
                    }
                });
            });
        }        



        public async Task<HistoricalRateResponse> GetHistoricalRatesAsync(
            string baseCurrency,
            string targetCurrency,
            DateTime from,
            DateTime to)
        {
            
            if(baseCurrency == "USD" && targetCurrency == "EUR")
            {
                var rates = new Dictionary<DateTime, decimal>
            {
                { new DateTime(2024, 12, 01), 1.09M },
                { new DateTime(2024, 12, 02), 1.05M },
                { new DateTime(2024, 12, 03), 1.11M },
                { new DateTime(2024, 12, 04), 1.250M },
                { new DateTime(2024, 12, 05), 1.095M }
            };

                return await ExecuteWithRetryAsync(() =>
                {
                    return Task.FromResult(new HistoricalRateResponse
                    {
                        Base = baseCurrency,
                        Target = targetCurrency,
                        Rates = rates
                        .Where(r => r.Key >= from && r.Key <= to)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    });
                });
            }

            else if(baseCurrency == "GBP" && targetCurrency == "EUR")
            {
                var rates = new Dictionary<DateTime, decimal>
            {
                { new DateTime(2024, 12, 10), 1.20M },
                { new DateTime(2024, 12, 12), 1.204M },
                { new DateTime(2024, 12, 13), 1.201M },
                { new DateTime(2024, 12, 14), 1.25M },
                { new DateTime(2024, 12, 15), 1.216M }
            };

                return await ExecuteWithRetryAsync(() =>
                {
                    return Task.FromResult(new HistoricalRateResponse
                    {
                        Base = baseCurrency,
                        Target = targetCurrency,
                        Rates = rates
                        .Where(r => r.Key >= from && r.Key <= to)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    });
                });
            }
            else if(baseCurrency == "USD" && targetCurrency == "GBP")
            {
                var rates = new Dictionary<DateTime, decimal>
            {
                { new DateTime(2024, 12, 27), 0.79M },
                { new DateTime(2024, 12, 28), 0.80M },
                { new DateTime(2024, 12, 29), 0.81M },
                { new DateTime(2022, 12, 30), 0.80M },
                { new DateTime(2024, 12, 31), 0.795M }
            };

                return await ExecuteWithRetryAsync(() =>
                {
                    return Task.FromResult(new HistoricalRateResponse
                    {
                        Base = baseCurrency,
                        Target = targetCurrency,
                        Rates = rates
                        .Where(r => r.Key >= from && r.Key <= to)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    });
                });
            }

            else
            {
                return new HistoricalRateResponse
                {
                    Base = "No Record",
                    Target = "No Record"
                };
            }
            
        }

        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (ex, time) =>
                    {
                        Console.WriteLine($"Retrying due to: {ex.Message}");
                    });

            return await retryPolicy.ExecuteAsync(action);
        }


    }

}
