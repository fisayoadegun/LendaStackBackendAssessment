using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Dto.Request
{
    public class HistoricalConvertRequest
    {
        public string BaseCurrency { get; set; } = default!;
        public string TargetCurrency { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
