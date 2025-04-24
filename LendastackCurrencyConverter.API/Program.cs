using AspNetCoreRateLimit;
using LendastackCurrencyConverter.Core.Interfaces;
using LendastackCurrencyConverter.Core.Services;
using LendastackCurrencyConverter.Infrastructure;
using LendastackCurrencyConverter.Infrastructure.Interface;
using LendastackCurrencyConverter.Infrastructure.Repository;
using LendastackCurrencyConverter.Worker;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(
    builder.Configuration.GetSection("ApiKeyRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddControllers();
builder.Services.AddScoped<ICurrencyRateService, SimulatedCurrencyRateService>();
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
builder.Services.AddHostedService<RealTimeRateFetcher>();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseIpRateLimiting();

app.Use(async (context, next) =>
{
    var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();
    var validKeys = builder.Configuration.GetSection("ApiKeys").Get<List<string>>();

    if (apiKey == null || !validKeys.Contains(apiKey))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key is missing or invalid.");
        return;
    }

    await next();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
