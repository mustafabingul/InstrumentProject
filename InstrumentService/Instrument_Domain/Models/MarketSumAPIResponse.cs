using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Domain.Models
{
    public class MarketSumAPIResponse
    {
        public Marketsummaryandsparkresponse marketSummaryAndSparkResponse { get; set; }

        public class Marketsummaryandsparkresponse
        {
            public Result[] result { get; set; }
            public object error { get; set; }
        }

        public class Result
        {
            public string fullExchangeName { get; set; }
            public string exchangeTimezoneName { get; set; }
            public string symbol { get; set; }
            public int gmtOffSetMilliseconds { get; set; }
            public long firstTradeDateMilliseconds { get; set; }
            public int exchangeDataDelayedBy { get; set; }
            public string language { get; set; }
            public Regularmarkettime regularMarketTime { get; set; }
            public string exchangeTimezoneShortName { get; set; }
            public string quoteType { get; set; }
            public string customPriceAlertConfidence { get; set; }
            public string marketState { get; set; }
            public string market { get; set; }
            public Spark spark { get; set; }
            public int priceHint { get; set; }
            public bool tradeable { get; set; }
            public int sourceInterval { get; set; }
            public string exchange { get; set; }
            public string shortName { get; set; }
            public string region { get; set; }
            public bool triggerable { get; set; }
            public Regularmarketpreviousclose regularMarketPreviousClose { get; set; }
        }

        public class Regularmarkettime
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class Spark
        {
            public int ? [] timestamp { get; set; }
            public int ? dataGranularity { get; set; }
            public string symbol { get; set; }
            public int? end { get; set; }
            public int ? start { get; set; }
            public float? previousClose { get; set; }
            public float?[] close { get; set; }
            public float? chartPreviousClose { get; set; }
        }

        public class Regularmarketpreviousclose
        {
            public float raw { get; set; }
            public string fmt { get; set; }
        }
    }
}
