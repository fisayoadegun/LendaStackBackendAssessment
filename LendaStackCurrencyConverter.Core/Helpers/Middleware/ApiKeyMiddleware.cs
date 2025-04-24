using LendastackCurrencyConverter.Core.Services.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Helpers.Middleware
{    
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiKeyValidator validator)
        {
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey) ||
                !validator.IsValid(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid or missing API Key.");
                return;
            }

            await _next(context);
        }
    }

}
