using GetInstrument_Job.Interface;
using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInstrument_Job.Services
{
    public class MarketSummaryService : IMarketSummaryService
    {
        private readonly ILogger<MarketSummaryService> _logger;
        private readonly IMarketSummaryDAO _marketSummaryDAO;
        private IConfiguration _config;

        public MarketSummaryService(ILogger<MarketSummaryService> logger, IConfiguration config, IMarketSummaryDAO marketSummaryDAO)
        {
            _logger = logger;
            _config = config;
            _marketSummaryDAO = marketSummaryDAO;   
        }
     
        public async Task<MarketSumAPIResponse> GetMarketSummaryAsync()
        {
            try
            {
                var client = new RestClient();
                var resource = _config.GetValue<string>("MarketBaseUrl");
                var apiKey = _config.GetValue<string>("API_Key");
                var apiHost = _config.GetValue<string>("API_Host");
                var request = new RestRequest(resource, Method.Get);
                request.AddHeader("X-RapidAPI-Key", apiKey);
                request.AddHeader("X-RapidAPI-Host", apiHost);
                var response = await client.ExecuteAsync(request);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        var resp = JsonConvert.DeserializeObject<MarketSumAPIResponse>(response.Content, settings);
                       
                        return resp;

                    case System.Net.HttpStatusCode.BadRequest:
                        Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
                        if (dict.ContainsKey("message"))
                        {
                            throw new Exception(dict["message"].ToString());
                        }
                        throw new Exception(response.ErrorMessage + ":" + response.ErrorException);
                    case System.Net.HttpStatusCode.InternalServerError:
                    case System.Net.HttpStatusCode.Unauthorized:
                    default:

                        throw new Exception(response.ErrorMessage + ":" + response.ErrorException);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ":" + ex.StackTrace);
                return null;
            }
        }

        public async Task<int> InsertMarketSummaryAsync()
        {
            int count = 0; 
            var result = new Result<MarketSummaryResponse>();   
            var marrketResponse = new MarketSummaryResponse();  
            try
            {
               var res = await AddMarketSummaryList();
                foreach (var item in res) 
                {
                    var check = await _marketSummaryDAO.CheckMarketSummay(item);
                    if (check == false)
                    {
                        result = await _marketSummaryDAO.Insert(item);
                        if (result.IdentityValue > 0)
                        {
                            _logger.LogInformation(result.IdentityValue.ToString());
                            count++;
                        }
                    }
                
                    
                            
                }

                return count;
             }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ":" + ex.StackTrace);
                return count;
            }
        }

        private async Task<List<MarketSummaryResponse>> AddMarketSummaryList()
        {
          
            List<MarketSummaryResponse> marketSummaryResponses = new List<MarketSummaryResponse>(); 
         
            try
            {
                var res = await GetMarketSummaryAsync();
                foreach (var response in res.marketSummaryAndSparkResponse.result.ToList())
                {
                    var marketSummary = new MarketSummaryResponse();
                    marketSummary.ShortName = response.shortName;
                    marketSummary.Type = response.quoteType;
                    marketSummary.Symbol = response.symbol;
                    marketSummaryResponses.Add(marketSummary);
                }

                return marketSummaryResponses;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message + ":" + ex.StackTrace);
                return marketSummaryResponses;
            }

        }
    }
}
