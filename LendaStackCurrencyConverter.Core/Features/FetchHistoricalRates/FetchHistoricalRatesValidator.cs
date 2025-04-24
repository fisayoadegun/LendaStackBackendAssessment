using FluentValidation;
using LendastackCurrencyConverter.Core.Features.FetchUpdateHistorycalRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Features.FetchHistoricalRates
{
    public class FetchHistoricalRatesValidator : AbstractValidator<FetchHistoricalRatesCommand>
    {
        public FetchHistoricalRatesValidator()
        {
            RuleFor(x => x.HistoricalRateRequest.BaseCurrency)
            .NotEmpty().WithMessage("Base currency is required.")
            .Length(3).WithMessage("Base currency must be exactly 3 characters.")
            .Matches("^[A-Z]+$").WithMessage("Base currency must contain only uppercase letters.");

            RuleFor(x => x.HistoricalRateRequest.TargetCurrency)
                .NotEmpty().WithMessage("Target currency is required.")
                .Length(3).WithMessage("Target currency must be exactly 3 characters.")
                .Matches("^[A-Z]+$").WithMessage("Target currency must contain only uppercase letters.");

            RuleFor(x => x.HistoricalRateRequest.StartDate)
                .LessThanOrEqualTo(x => x.HistoricalRateRequest.EndDate)
                .WithMessage("Start date must be earlier than or equal to end date.");

            RuleFor(x => x.HistoricalRateRequest.EndDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("End date cannot be in the future.");
        }
    }
}
