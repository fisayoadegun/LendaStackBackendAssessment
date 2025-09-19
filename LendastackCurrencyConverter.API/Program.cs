using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using LendastackCurrencyConverter.Core.Helpers.Middleware;
using LendastackCurrencyConverter.Core.Interfaces;
using LendastackCurrencyConverter.Core.Services;
using LendastackCurrencyConverter.Core.Services.Security;
using LendastackCurrencyConverter.Infrastructure;
using LendastackCurrencyConverter.Infrastructure.Interface;
using LendastackCurrencyConverter.Infrastructure.Repository;
using LendastackCurrencyConverter.Worker;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
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
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddScoped<ICurrencyRateService, SimulatedCurrencyRateService>();
        builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        builder.Services.AddHostedService<RealTimeRateFetcher>();
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        builder.Services.AddEndpointsApiExplorer();
        // Swagger Documentation Section
        var info = new OpenApiInfo()
        {
            Title = "Lendastack Currency Converter API",
            Version = "v1",
            Description = "API for converting currencies using real-time exchange rates.",
            Contact = new OpenApiContact()
            {
                Name = "Fisayo Adegun",
                Email = "fisayoadegun@gmail.com"
            }
        };
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lendastack Currency Converter API", Version = "v1" });
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key needed to access the endpoints.",
                In = ParameterLocation.Header,
                Name = "x-api-key",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
            });
        });


        builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File("logs/lendastackcurrencyconverter-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        builder.Logging.AddConsole();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(u =>
            {
                u.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Lendastack Currency Converter");
            });
        }

        app.UseIpRateLimiting();


        app.UseHttpsRedirection();

        app.UseAuthorization();        

        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI();        

        app.Run();
    }

}