using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Instrument_Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Instrument_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockSummaryController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockSummaryController> _logger;
        private readonly ICacheService _cacheService;
      


        public StockSummaryController(IStockService stockService, ILogger<StockSummaryController> logger, ICacheService cacheService)
        {
            _logger = logger;
            _stockService = stockService;
            _cacheService = cacheService;
         
           
        }

        [HttpGet]
        [Route("ListAllStockSummary")]
        public async Task<IActionResult> ListAllStockSummary([FromQuery]int pageno, int pageSize)
        {
            try
            {
                if (pageno == 0 && pageSize == 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Errors = "Invalid Pagenumber and Pagesize"
                    });
                }
                if (pageno == 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Errors = "Invalid Pagenumber"
                    });
                }
                if (pageSize == 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Errors = "Invalid Pagesize"
                    });
                }
                var result = await _stockService.GetAllAsync(pageno, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                 return StatusCode(500);    
            }            
        }

        [HttpGet("GetPrice")]
      
        public async Task<IActionResult> GetInstrumentPrice()
        {
            try
            {
                var key = "price";
                var result = await _cacheService.GetCacheValue(key);
             
           
                return string.IsNullOrEmpty(result) ? (IActionResult) NotFound() : Ok(new SuccessResponse { Success = true, Message = result, ResponseCode= "00"});
           
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }

        [HttpPost]
        [Route("createAlert")]
        public async Task<IActionResult> CreateAlert([FromBody] AlertRequest alertRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Errors = "Invalid Request"
                    });
                }
                bool check = await _stockService.CheckStockSummarybyIdAsync(alertRequest.StockId);
                if (check)
                {
                    var result = await _stockService.CreateAlert(alertRequest);
                    if (result.IdentityValue > 0)
                    {
                      
                        return Ok(new SuccessResponse { 
                          ResponseCode ="00",
                          Message="Subcribed Successfully",
                          Success = true
                        });
                    }
                    return BadRequest(new ErrorResponse
                    {
                        Errors = "Invalid Alert Request"
                    });
                }
                else
                {
                    return Ok(new SuccessResponse
                    {
                        ResponseCode ="99",
                        Message = "The Stock requested for is not available at the momemnt",
                        Success= false
                    });
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
