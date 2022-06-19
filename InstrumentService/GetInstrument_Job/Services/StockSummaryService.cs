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
    public class StockSummaryService : IStockSummaryService
    {
        private readonly ILogger<StockSummaryService> _logger;
        private readonly IStockSummaryDAO _stockSummaryDAO;
        private IConfiguration _config;
        private readonly IMessageProducer _messagePublisher;
        private readonly IRedisCacheService _cacheService;

        public StockSummaryService(ILogger<StockSummaryService> logger, IConfiguration config, IStockSummaryDAO stockSummaryDAO, IRedisCacheService cacheService, IMessageProducer messagePublisher)
        {
            _logger = logger;
            _config = config;
            _stockSummaryDAO = stockSummaryDAO;
            _cacheService = cacheService;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result<AlertRequest>> GetAlertRequest()
        {
            return await _stockSummaryDAO.GetAlertRequest();
        }    

        public async Task<StockSumAPIResponse> GetStockSummaryAsync()
        {
            try
            {
                var client = new RestClient();
                var resource = _config.GetValue<string>("StockBaseUrl");
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
                        var resp = JsonConvert.DeserializeObject<StockSumAPIResponse>(response.Content, settings);
                       
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

        public async Task<int> InsertStockSummaryAsync()
        {
            int count = 0; 
            var result = new Result<StockSummaryResponse>();   
            var stockResponse = new StockSummaryResponse();  
            try
            {
                var res = await AddStockSummary();
                var cachekey = "price";
                _logger.LogInformation(res.Price.ToString());
                await _cacheService.SetCacheValueAsync(cachekey, res.Price.ToString());
                var check = await _stockSummaryDAO.CheckStockSummary(res);
                    if (check == false)
                    {
                        result = await _stockSummaryDAO.Insert(res);
                        if (result.IdentityValue > 0)
                        {
                            _logger.LogInformation(result.IdentityValue.ToString());
                            count++;
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

        private async Task<StockSummaryResponse> AddStockSummary()
        {          
            List<StockSummaryResponse> stockSummaryResponses = new List<StockSummaryResponse>(); 
         
            try
            {               
                var res = await GetStockSummaryAsync();
                var stockSummary = new StockSummaryResponse
                 {
                  ShortName = res.price.shortName,
                  LongName = res.price.longName,
                  Symbol = res.symbol,
                  Currency = res.price.currency,
                  CurrencySymbol = res.price.currencySymbol,
                  ExchangeName = res.price.exchangeName,
                  Price = (decimal)res.price.regularMarketPrice.raw,
                  Summary = res.summaryProfile.longBusinessSummary,
                  Recommendation = res.financialData.recommendationKey
               };
                return stockSummary;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message + ":" + ex.StackTrace);
                return null;
            }

        }

        public async Task<List<AlertRequest>> PushAlertRequest()
        {
            var setInitialPrice = 0;
            List<AlertRequest> alertRequests = new List<AlertRequest>();
            try
            {
                var res = await GetAlertRequest();
               
                if (res.List.Any())
                {
                    foreach (var alert in res.List)
                    {
                        var key = "price";
                        var getprice = await _cacheService.GetCacheValueAsync(key);  
                        var price = Convert.ToDecimal(getprice);
                           
                        if (alert.Price == price)
                        {
                            var update = await _stockSummaryDAO.UpdateAlertStatusById(true, alert.Id);
                            _messagePublisher.SendMessage(alert);
                        }
                        
                           
                    }

                    return alertRequests;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ":" + ex.StackTrace);
                return null;
            }
        }

        public async Task<int> UpdateAlertStatusById(bool status, long id)
        {
            try
            {

                var res = await _stockSummaryDAO.UpdateAlertStatusById(status, id);

                return res;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message + ":" + ex.StackTrace);
                return 0;
            }

        }
    }
}
