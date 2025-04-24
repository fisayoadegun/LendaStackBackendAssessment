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

        public async Task<ExchangeRate> GetRealTimeExchangeRate(string baseCurrency, string targetCurrency)
        {
            return await _context.ExchangeRates.Where(x => x.BaseCurrency == baseCurrency 
                        && x.TargetCurrency == targetCurrency 
                        && x.IsRealTime)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();            
        }
    }
}
