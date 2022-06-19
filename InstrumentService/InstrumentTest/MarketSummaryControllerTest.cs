using Castle.Core.Logging;
using Instrument_API.Controllers;
using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Net;
using System.Threading.Tasks;

namespace InstrumentTest
{
    public class MarketSummaryControllerTest
    {
        private MarketSummaryController marketSummaryController;

        private Mock<IMarketService> marketServiceMock;
        private ILogger<MarketSummaryController> loggerServiceMock;
        private Result<MarketSummaryResponse> result;

        [SetUp]
        public void Setup()
        {
            marketServiceMock = new Mock<IMarketService>();

            var mockResult = new Mock<Result<MarketSummaryResponse>>();

            result = new Result<MarketSummaryResponse>() { 
                Data = mockResult.Object.Data
            };

            marketServiceMock.Setup(m => m.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new Result<MarketSummaryResponse>()));
            marketSummaryController = new MarketSummaryController(marketServiceMock.Object, loggerServiceMock);        
        }

        [Test]
        public async Task ShouldReturnList()
        {
            var res = await marketSummaryController.ListAllMarketSummary(1,1);
            Assert.IsNotNull(res);
        }
    }
}