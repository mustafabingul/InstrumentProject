using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Instrument_API.Service
{
    public class MarketService : IMarketService
    {
         private readonly IMarketSummaryDAO _marketSummaryDAO;
         private ILogger<MarketService> _logger;

        public MarketService(IMarketSummaryDAO marketSummaryDAO, ILogger<MarketService> logger)
        {
            _marketSummaryDAO = marketSummaryDAO;
            _logger = logger;
        }

        public async Task<Result<MarketSummaryResponse>> GetAllAsync(int pageNum, int pageSize)
        {
            return await _marketSummaryDAO.GetAllMarketSummaryDAO(pageNum, pageSize);
        }
    }
}
