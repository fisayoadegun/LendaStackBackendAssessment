using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Features.ConvertHistoricalRate
{
    public class ConvertHistoricalRateValidator : AbstractValidator<ConvertHistoricalRateCommand>
    {
        public ConvertHistoricalRateValidator()
        {
            RuleFor(x => x.HistoricalConvertRequest.BaseCurrency)
                .NotEmpty().WithMessage("Base currency is required.")
                .Length(3).WithMessage("Base currency must be exactly 3 characters.")
                .Matches("^[A-Z]+$").WithMessage("Base currency must be uppercase letters.");

            RuleFor(x => x.HistoricalConvertRequest.TargetCurrency)
                .NotEmpty().WithMessage("Target currency is required.")
                .Length(3).WithMessage("Target currency must be exactly 3 characters.")
                .Matches("^[A-Z]+$").WithMessage("Target currency must be uppercase letters.");

            RuleFor(x => x.HistoricalConvertRequest.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.HistoricalConvertRequest.Date)
                .NotEqual(default(DateTime)).WithMessage("Date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date must not be in the future.");
        }
    }
}
