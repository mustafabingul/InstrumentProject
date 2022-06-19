using Instrument_Application.Interface;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

      
        public async Task<string> GetCacheValue(string key)
        {
           var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);    
        }

    
    }
}
