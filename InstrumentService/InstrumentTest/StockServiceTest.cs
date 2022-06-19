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
    public class StockServiceTest
    {
        private StockService stockService;

        private Mock<IStockSummaryDAO> stockSummaryDAOMock;
        private ILogger<StockService> loggerService;

        [SetUp]
        public void Setup()
        {
            stockSummaryDAOMock = new Mock<IStockSummaryDAO>();

            var mockResult = new Mock<Result<StockSummaryResponse>>();

            var reslt = new Result<AlertRequest>
            {
                Data = new AlertRequest { 
                    Email = "bnglmstf@gmail.com",
                    Price = (decimal)1.83
                }
            };

            stockSummaryDAOMock.Setup(s => s.GetAllStockSummaryDAO(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new Result<StockSummaryResponse>()));
            stockSummaryDAOMock.Setup(s => s.CreateAlert(It.IsAny<AlertRequest>())).Returns(Task.FromResult(reslt));
            stockSummaryDAOMock.Setup(s => s.GetAlertRequest()).Returns(Task.FromResult(reslt));

            stockService = new StockService(stockSummaryDAOMock.Object, loggerService);        
        }

        [Test]
        public async Task ShouldReturnCreatedAlertRequest()
        {
            var reslt = new Result<AlertRequest>
            {
                Data = new AlertRequest
                {
                    Email = "bnglmstf@gmail.com",
                    Price = (decimal)1.83
                }
            };

            var res = await stockService.CreateAlert(new AlertRequest());
            Assert.AreEqual(reslt.Data.Email, res.Data.Email);
            Assert.AreEqual(reslt.Data.Price, res.Data.Price);
        }
    }
}