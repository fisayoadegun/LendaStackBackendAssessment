using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LendastackCurrencyConverter.Core.Dto;

namespace LendastackCurrencyConverter.Core.Features.ConvertCurrency
{
    public class ConvertCurrencyValidator : AbstractValidator<ConvertCurrencyCommand>
    {
        public ConvertCurrencyValidator()
        {
            RuleFor(x => x.ConvertRequest.BaseCurrency)
                .NotEmpty().WithMessage("Base currency is required.")
                .Length(3).WithMessage("Base currency must be 3 characters.")
                .Matches("^[A-Z]+$").WithMessage("Base currency must be uppercase letters.");

            RuleFor(x => x.ConvertRequest.TargetCurrency)
                .NotEmpty().WithMessage("Target currency is required.")
                .Length(3).WithMessage("Target currency must be 3 characters.")
                .Matches("^[A-Z]+$").WithMessage("Target currency must be uppercase letters.");

            RuleFor(x => x.ConvertRequest.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }

}
