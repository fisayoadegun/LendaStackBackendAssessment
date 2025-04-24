using LendastackCurrencyConverter.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendaStackCurrencyConverter.Tests.UnitTests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly SimulatedCurrencyRateService _service;

        public ExchangeRateServiceTests()
        {
            _service = new SimulatedCurrencyRateService(); // No dependencies in this mock service
        }

        [Fact]
        public async Task GetRealTimeRatesAsync_ShouldReturnRates()
        {
            var result = await _service.GetRealTimeRatesAsync("USD");

            Assert.NotNull(result);
            Assert.True(result.Rates.ContainsKey("EUR"));
            Assert.True(result.Rates["EUR"] > 0);
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_ShouldReturnExpectedDates()
        {
            var start = new DateTime(2024, 12, 1);
            var end = new DateTime(2025, 4, 5);

            var result = await _service.GetHistoricalRatesAsync("USD", "GBP", start, end);
            var expectedDates = new List<string>();
            foreach(var date in result.Rates.Keys)
            {
                expectedDates.Add(date.ToString("yyyy-MM-dd"));
            }

            Assert.Equal(5, result.Rates.Count);
            Assert.Contains("2024-12-27", expectedDates);
        }
    }
}
