using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInstrument_Job.Interface
{
    public interface IMarketSummaryService
    {
    
        Task<MarketSumAPIResponse> GetMarketSummaryAsync();
     
        Task<int> InsertMarketSummaryAsync();
    }
}
