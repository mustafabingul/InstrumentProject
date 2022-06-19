using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Application.Interface
{
    public interface IMarketSummaryDAO
    {
        Task<Result<MarketSummaryResponse>>Insert(MarketSummaryResponse marketSummaryResponse); 
        Task <bool> CheckMarketSummay(MarketSummaryResponse marketSummaryResponse);
        Task<Result<MarketSummaryResponse>> GetAllMarketSummaryDAO(int pageNum, int PageSize);
        
    }
}
