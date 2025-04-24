using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Services.Security
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
