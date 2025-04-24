using LendastackCurrencyConverter.Infrastructure.Interface;
using LendastackCurrencyConverter.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LendastackCurrencyConverter.Infrastructure.Repository
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly AppDbContext _context;
        public ExchangeRateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExchangeRate> AddExchangeRate(ExchangeRate exchangeRate)
        {
            try
            {
                await _context.ExchangeRates.AddAsync(exchangeRate);
                await _context.SaveChangesAsync();
                return exchangeRate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ExchangeRate> GetHistoricalExchangeRate(string baseCurrency, string targetCurrency, DateTime date)
        {
            return await _context.ExchangeRates
                .Where(r => r.BaseCurrency == baseCurrency
                         && r.TargetCurrency == targetCurrency
                         && r.Date == date                         
                         && !r.IsRealTime)
                .OrderBy(r => r.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ExchangeRate>> GetHistoricalRates(string baseCurrency, string targetCurrency, DateTime startDate, DateTime endDate)
        {
             return await _context.ExchangeRates
                .Where(r => r.BaseCurrency == baseCurrency
                         && r.TargetCurrency == targetCurrency
                         && r.Date >= startDate
                         && r.Date <= endDate
                         && !r.IsRealTime)
                .OrderBy(r => r.Date)
                .ToListAsync();
        }

        public async Task<ExchangeRate> GetRealTimeExchangeRate(string baseCurrency, string targetCurrency)
        {
            var rates = await _context.ExchangeRates.Where(x => x.BaseCurrency == baseCurrency 
                        && x.TargetCurrency == targetCurrency 
                        && x.IsRealTime)
                .OrderByDescending(x => x.Date).ToListAsync();
            if (rates.Any())
            {
                var maxId = rates.Max(x => x.Id);

                return rates.Where(x => x.Id == maxId).FirstOrDefault();
            }

            else
                return null;
        }

        public async Task<bool> ExistsAsync(string baseCurrency, string targetCurrency, DateTime date)
        {
            return await _context.ExchangeRates.AnyAsync(x =>
                x.BaseCurrency == baseCurrency &&
                x.TargetCurrency == targetCurrency &&
                x.Date == date);
        }

    }
}
