using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Application.Interface
{
    public interface IStockSummaryDAO
    {
        Task<Result<StockSummaryResponse>>Insert(StockSummaryResponse stockSummaryResponse); 
        Task <bool> CheckStockSummary(StockSummaryResponse stockSummaryResponse);
        Task<bool> CheckStockSummarybyId(long id);
        Task<Result<AlertRequest>> GetAlertRequest();
        Task<int> UpdatePrice(decimal price, long id);
        Task<int> UpdateAlertStatusById(bool status, long id);
        Task<Result<AlertRequest>> CreateAlert(AlertRequest alertRequest);
        Task<Result<StockSummaryResponse>> GetAllStockSummaryDAO(int pageNum, int PageSize);        
    }
}
