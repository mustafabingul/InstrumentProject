using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Domain.Models
{
    public class MarketSummaryResponse
    {
        public long AppId { get; set; }
        public string Symbol { get; set; }
        public string ShortName  { get; set; }
        public string Type { get; set; }    
    }
}
