using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Application.Interface
{
    public interface IMarketService
    {
        Task<Result<MarketSummaryResponse>> GetAllAsync(int pageNum, int pageSize);
    }
}
