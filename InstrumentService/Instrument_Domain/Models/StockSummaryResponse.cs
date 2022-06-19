using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Domain.Models
{
    public class StockSummaryResponse
    {
        public long AppId { get; set; }

        public string Symbol { get; set; }
        public string CurrencySymbol { get; set; }
        public string Currency { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string ExchangeName { get; set; }
        public decimal Price { get; set; }
        public string Summary { get; set; }
        public string Recommendation { get; set; }
    }
}
