using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendastackCurrencyConverter.Core.Dto.Response
{
    public class HistoricalRateResponse
    {
        public string Base { get; set; }
        public string Target { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public Dictionary<DateTime, decimal> Rates { get; set; }
    }
}
