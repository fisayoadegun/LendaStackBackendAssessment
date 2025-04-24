using LendastackCurrencyConverter.Core.Dto.Request;
using LendastackCurrencyConverter.Core.Dto.Response;
using LendastackCurrencyConverter.Core.Features.ConvertCurrency;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LendaStackCurrencyConverter.Tests.IntegrationTests.Services
{
    public class RealTimeConversionTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RealTimeConversionTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["ApiKeys:0"] = "test-key-123"
                    });
                });
            }).CreateClient();

            _client.DefaultRequestHeaders.Add("X-Api-Key", "test-key-123");
        }

        [Fact]
        public async Task ConvertRealTime_ReturnsExpectedResult()
        {
            var request = new ConvertRequest
            {
                BaseCurrency = "USD",
                TargetCurrency = "EUR",
                Amount = 200
            };

            var payload = new ConvertCurrencyCommand
            {
                ConvertRequest = request,
            };

            var response = await _client.PostAsJsonAsync("/api/v1/convert/convert-real-time-rate", payload);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<ExchangeRateResponseDto>>();

            Assert.NotNull(result);
            Assert.True(result.Data.ConvertedAmount > 0);
        }
    }

}
