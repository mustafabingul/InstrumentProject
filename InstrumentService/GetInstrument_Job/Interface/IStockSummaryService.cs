using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInstrument_Job.Interface
{
    public interface IStockSummaryService
    {
        Task<StockSumAPIResponse> GetStockSummaryAsync();
        Task<Result<AlertRequest>> GetAlertRequest();
        Task<List<AlertRequest>> PushAlertRequest();
        Task<int> InsertStockSummaryAsync();
        Task<int> UpdateAlertStatusById(bool status, long id);
    }
}
