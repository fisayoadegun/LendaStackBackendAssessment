using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Services.Security
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IConfiguration _config;

        public ApiKeyValidator(IConfiguration config)
        {
            _config = config;
        }

        public bool IsValid(string apiKey)
        {
            var validKeys = _config.GetSection("ApiKeys").Get<List<string>>() ?? new();
            return validKeys.Contains(apiKey);
        }
    }
}
