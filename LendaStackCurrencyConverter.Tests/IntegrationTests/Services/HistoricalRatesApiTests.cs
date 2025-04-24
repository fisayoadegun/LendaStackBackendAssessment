using LendastackCurrencyConverter.Core.Dto.Request;
using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Features.ConvertHistoricalRate;
using LendastackCurrencyConverter.Core.Features.FetchUpdateHistorycalRates;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;


namespace LendaStackCurrencyConverter.Tests.IntegrationTests.Services
{
    public class HistoricalRatesApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public HistoricalRatesApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ApiKeys:0"] = "test-key-123"
                    });
                });
            }).CreateClient();

            _client.DefaultRequestHeaders.Add("X-Api-Key", "test-key-123");
        }

        [Fact]
        public async Task FetchUpdateHistoricalRates_ReturnsSuccess()
        {
            var request = new HistoricalRateRequest
            {
                BaseCurrency = "USD",
                TargetCurrency = "GBP",
                StartDate = DateTime.Parse("2024-12-01"),
                EndDate = DateTime.Parse("2025-04-05")
            };

            var payload = new FetchUpdateHistoricalRateCommand
            {
                HistoricalRateRequest = request,
            };

            var response = await _client.PostAsJsonAsync("/api/v1/historical/fetch-and-update-historical-rates", payload);

            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<HistoricalRateResponse>>();            
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task FetchHistoricalRates_ReturnsSuccess()
        {
            // Arrange           
            var baseCurrency = "USD";
            var targetCurrency = "GBP";
            var startDate = new DateTime(2024, 12, 27).ToString("yyyy-MM-dd");
            var endDate = new DateTime(2024, 12, 31).ToString("yyyy-MM-dd");

            var url = $"/api/v1/historical/get-historical-rates?baseCurrency={baseCurrency}&targetCurrency={targetCurrency}&startDate={startDate}&endDate={endDate}";

            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Will throw if not 2xx

            var content = await response.Content.ReadFromJsonAsync<BaseResponse<List<ExchangeRateResponseDto>>>();

            Assert.NotNull(content);
            Assert.True(content.Success);
            Assert.Equal(baseCurrency, content.Data.FirstOrDefault().BaseCurrency);
            Assert.Equal(targetCurrency, content.Data.FirstOrDefault().TargetCurrency);
            Assert.NotNull(content);
            Assert.True(content.Data.Any());

        }

        [Fact]
        public async Task ConvertWithHistoricalRate_ReturnsConvertedValue()
        {            
            var request = new HistoricalConvertRequest
            {
                BaseCurrency = "USD",
                TargetCurrency = "GBP",
                Amount = 100,
                Date = DateTime.Parse("2024-12-31")
            };

            var payload = new ConvertHistoricalRateCommand
            {
                HistoricalConvertRequest = request,
            };

            var response = await _client.PostAsJsonAsync("/api/v1/convert/historical/convert-historical-rate", payload);

            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<ExchangeRateResponseDto>>();
            Assert.True(result.Data.ConvertedAmount > 0);
        }
    }
}
