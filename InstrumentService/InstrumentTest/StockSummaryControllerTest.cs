using Castle.Core.Logging;
using Instrument_API.Controllers;
using Instrument_Application.Interface;
using Instrument_Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace InstrumentTest
{
    public class StockSummaryControllerTest
    {
        private StockSummaryController stockSummaryController;

        private Mock<IStockService> stockServiceMock;
        private Mock<ICacheService> cacheServiceMock;
        private ILogger<StockSummaryController> loggerService;
        private Result<MarketSummaryResponse> result;

        [SetUp]
        public void Setup()
        {
            stockServiceMock = new Mock<IStockService>();
            cacheServiceMock = new Mock<ICacheService>();

            var mockResult = new Mock<Result<StockSummaryResponse>>();

            stockServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new Result<StockSummaryResponse>()));
            stockSummaryController = new StockSummaryController(stockServiceMock.Object, loggerService, cacheServiceMock.Object);        
        }

        [Test]
        public async Task ShouldReturnResponse()
        {
            var res = await stockSummaryController.ListAllStockSummary(1, 1);
            Assert.IsNotNull(res);
        }
    }
}