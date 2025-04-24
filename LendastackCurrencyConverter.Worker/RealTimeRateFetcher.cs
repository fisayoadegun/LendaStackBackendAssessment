using LendastackCurrencyConverter.Core.Interfaces;
using LendastackCurrencyConverter.Infrastructure.Models;
using LendastackCurrencyConverter.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using LendastackCurrencyConverter.Infrastructure.Interface;

namespace LendastackCurrencyConverter.Worker
{
    public class RealTimeRateFetcher : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RealTimeRateFetcher> _logger;        

        public RealTimeRateFetcher(IServiceProvider serviceProvider, ILogger<RealTimeRateFetcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var rateService = scope.ServiceProvider.GetRequiredService<ICurrencyRateService>();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    var rates = await rateService.GetRealTimeRatesAsync("USD");
                    foreach (var rate in rates.Rates)
                    {                        
                        var duplicate =  db.ExchangeRates.Where(x => 
                            x.BaseCurrency == "USD" &&
                            x.TargetCurrency ==  rate.Key &&
                            x.Date ==  DateTime.UtcNow.Date
                        ).FirstOrDefault();

                        if (duplicate != null)
                            continue;

                        var entry = new ExchangeRate
                        {
                            BaseCurrency = "USD",
                            TargetCurrency = rate.Key,
                            Rate = rate.Value,
                            Date = DateTime.UtcNow.Date,
                            IsRealTime = true
                        };
                        db.ExchangeRates.Add(entry);
                    }

                    await db.SaveChangesAsync();
                    _logger.LogInformation("Real-time rates saved.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching real-time rates.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // hourly
            }
        }
    }
}
