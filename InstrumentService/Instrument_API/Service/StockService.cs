using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Instrument_API.Service
{
    public class StockService : IStockService
    {
         private readonly IStockSummaryDAO _stockSummaryDAO;
         private ILogger<StockService> _logger;

        public StockService(IStockSummaryDAO stockSummaryDAO, ILogger<StockService> logger)
        {
            _stockSummaryDAO = stockSummaryDAO;
            _logger = logger;
        }

        public async Task<bool> CheckStockSummarybyIdAsync(long id)
        {
            return await _stockSummaryDAO.CheckStockSummarybyId(id);
        }

        public async Task<Result<AlertRequest>> CreateAlert(AlertRequest alertRequest)
        {
            return await _stockSummaryDAO.CreateAlert(alertRequest);
        }       

        public async Task<Result<StockSummaryResponse>> GetAllAsync(int pageNum, int pageSize)
        {
            return await _stockSummaryDAO.GetAllStockSummaryDAO(pageNum, pageSize);
        }
    }
}
