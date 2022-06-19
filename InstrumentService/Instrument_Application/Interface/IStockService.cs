using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Application.Interface
{
    public interface IStockService
    {
        Task<Result<StockSummaryResponse>> GetAllAsync(int pageNum, int pageSize);
        Task<bool> CheckStockSummarybyIdAsync(long id);     
        Task<Result<AlertRequest>> CreateAlert(AlertRequest alertRequest);
    }
}
