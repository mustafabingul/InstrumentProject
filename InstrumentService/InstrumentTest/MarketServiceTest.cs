using Castle.Core.Logging;
using Instrument_API.Controllers;
using Instrument_API.Service;
using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace InstrumentTest
{
    public class MarketServiceTest
    {
        private MarketService marketService;

        private Mock<IMarketSummaryDAO> marketSummaryDAOMock;
        private ILogger<MarketService> loggerService;

        [SetUp]
        public void Setup()
        {
            marketSummaryDAOMock = new Mock<IMarketSummaryDAO>();

            var mockResult = new Mock<Result<MarketSummaryResponse>>();
            var reslt = new Result<MarketSummaryResponse>{ 
                Data = new MarketSummaryResponse() { 
                    Symbol = "Symbol"
                }
            };

            marketSummaryDAOMock.Setup(s => s.GetAllMarketSummaryDAO(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(reslt));
            
            marketService = new MarketService(marketSummaryDAOMock.Object, loggerService);        
        }

        [Test]
        public async Task ShouldReturnSameResponse()
        {
            var res = await marketService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>());
            Assert.AreEqual(res.Data.Symbol, "Symbol");
        }
    }
}