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
    public class MarketSummaryDAO : DbConfig, IMarketSummaryDAO
    {
        
        public MarketSummaryDAO(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<bool> CheckMarketSummay(MarketSummaryResponse marketSummaryResponse)
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();                
                    parameters.Add("Symbol", marketSummaryResponse.Symbol);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.QueryAsync("sp_check_marketsummary", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    Result<MarketSummaryResponse> result = new Result<MarketSummaryResponse>()
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
                Console.WriteLine("CheckMarketSummay", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return false;
            }
        }

        public async Task<Result<MarketSummaryResponse>> GetAllMarketSummaryDAO(int pageNum, int PageSize)
        {
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("PageNumber", pageNum);
                    parameters.Add("PageSize", PageSize);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.QueryAsync<MarketSummaryResponse>("sp_getall_marketsummary", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    {
                        Result<MarketSummaryResponse> result = new Result<MarketSummaryResponse>()
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
                Console.WriteLine("GetAllMarketSummaryDAO", ex.Source, ex);
                throw;
            }
        }

        public async Task<Result<MarketSummaryResponse>> Insert(MarketSummaryResponse marketSummaryResponse)
        {           
            try
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ShortName", marketSummaryResponse.ShortName);
                    parameters.Add("Symbol", marketSummaryResponse.Symbol);
                    parameters.Add("Type", marketSummaryResponse.Type);
         
                    parameters.Add("AppId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("Return_Val", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                    var dbResult = await conn.ExecuteAsync("sp_insert_marketsummary", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    Result<MarketSummaryResponse> result = new Result<MarketSummaryResponse>()
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
                Console.WriteLine("InsertMarketSummary", ex.Source.ToString() + "\r\n" + ex.StackTrace.ToString() + "\r\n" + ex.InnerException.ToString(), ex);
                return null;

            }
        }
    }
}
