using GetInstrument_Job.Interface;
using Instrument_Application.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInstrument_Job.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
             await db.StringSetAsync(key, value);
        }
    }
}
