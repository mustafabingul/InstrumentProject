using Dapper;
using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Instrument_Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Infrastructure.DAO
{
    public class StockSummaryDAO : DbConfig, IStockSummaryDAO
    {
        
        public StockSummaryDAO(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<bool> CheckStockSummary(StockSummaryResponse stockSummaryResponse)
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Symbol", stockSummaryResponse.Symbol);                    
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.QueryAsync("sp_check_stocksummary", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    Result<StockSummaryResponse> result = new Result<StockSummaryResponse>()
                    {  
                        ReturnValue = parameters.Get<Int32>("Return_Val"),
                    };
                    conn.Close();
                    if (result.ReturnValue == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CheckStockSummary", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return false;
            }
        }

        public async Task<bool> CheckStockSummarybyId(long id)
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Id", id);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.QueryAsync("sp_getstocksummary_id", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    Result<StockSummaryResponse> result = new Result<StockSummaryResponse>()
                    {
                        ReturnValue = parameters.Get<Int32>("Return_Val"),
                    };
                    conn.Close();
                    if (result.ReturnValue == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CheckStockSummary", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return false;
            }
        }

        public async Task<Result<AlertRequest>> CreateAlert(AlertRequest alertRequest)
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Email", alertRequest.Email);
                    parameters.Add("Status", alertRequest.Status);
                    parameters.Add("Price", alertRequest.Price);
                    parameters.Add("StockId", alertRequest.StockId);
                    parameters.Add("Id", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.ExecuteAsync("sp_createalert", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    Result<AlertRequest> result = new Result<AlertRequest>()
                    {
                        IdentityValue = parameters.Get<Int64>("Id"),
                        ReturnValue = parameters.Get<Int32>("Return_Val"),
                    };
                    conn.Close();
                    return await Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateAlert", ex.Source, ex);
                return null;
            }
        }

        public async  Task<Result<AlertRequest>> GetAlertRequest()
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Status", false);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.QueryAsync<AlertRequest>("sp_getalertstatus", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    Result<AlertRequest> result = new Result<AlertRequest>()
                    {
                        List = dbResult.ToList(),

                        
                    };
                    conn.Close();
                    return result;
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAlertRequest", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return null;
            }
        }

        public async Task<Result<StockSummaryResponse>> GetAllStockSummaryDAO(int pageNum, int PageSize)
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("PageNumber", pageNum);
                    parameters.Add("PageSize", PageSize);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.QueryAsync<StockSummaryResponse>("sp_getall_stocksummary", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    {
                        Result<StockSummaryResponse> result = new Result<StockSummaryResponse>()
                        {
                            List = dbResult.ToList(),
                            PageNumber = pageNum,
                            PageSize = PageSize,
                            NoOfRecords = dbResult.ToList().Count,
                            ReturnValue = parameters.Get<Int32>("Return_Val"),
                        };
                        conn.Close();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllStockSummaryDAO", ex.Source, ex);
                throw;
            }
        }

        public async Task<Result<StockSummaryResponse>> Insert(StockSummaryResponse stockSummaryResponse)
        {           
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ShortName", stockSummaryResponse.ShortName);
                    parameters.Add("LongName", stockSummaryResponse.LongName);
                    parameters.Add("Symbol", stockSummaryResponse.Symbol);
                    parameters.Add("Currency", stockSummaryResponse.Currency);
                    parameters.Add("CurrencySymbol", stockSummaryResponse.CurrencySymbol);
                    parameters.Add("ExchangeName", stockSummaryResponse.ExchangeName);
                    parameters.Add("Price", stockSummaryResponse.Price);
                    parameters.Add("Summary", stockSummaryResponse.Summary);
                    parameters.Add("Recommendation", stockSummaryResponse.Recommendation);
         
                    parameters.Add("AppId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.ExecuteAsync("sp_insert_stocksummary", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    Result<StockSummaryResponse> result = new Result<StockSummaryResponse>()
                    {
                        IdentityValue = parameters.Get<Int64>("AppId"),
                        ReturnValue = parameters.Get<Int32>("Return_Val"),
                    };
                    conn.Close();
                    return await Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddStockSummary", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return null;

            }
        }

        public async Task<int> UpdatePrice(decimal price, long id)
        {
            //
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Price", price);
                    parameters.Add("AppId", id);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.ExecuteAsync("sp_update_price", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return dbResult;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("AddStockSummary", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return 0;
            }
        }

        public async Task<int> UpdateAlertStatusById(bool status, long id)
        {
            //
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("Status", status);
                    parameters.Add("Id", id);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.ExecuteAsync("sp_updatealertby_id", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return dbResult;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("UpdateAlertStatusById", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return 0;
            }
        }
    }
}
