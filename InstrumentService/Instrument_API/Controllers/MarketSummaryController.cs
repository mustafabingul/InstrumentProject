using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Instrument_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketSummaryController : ControllerBase
    {
        private readonly IMarketService _marketService;
        private readonly ILogger<MarketSummaryController> _logger;

        public MarketSummaryController(IMarketService marketService, ILogger<MarketSummaryController> logger)
        {
            _logger = logger;
            _marketService = marketService;
        }
        [HttpGet]
        [Route("ListAllMarketSummary")]
        public async Task<IActionResult> ListAllMarketSummary([FromQuery]int pageno, int pageSize)
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
                    return BadRequest("Please input Page Number");
                }
                var result = await _marketService.GetAllAsync(pageno, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                 return StatusCode(500);    
            }
            
        }
    }
}
