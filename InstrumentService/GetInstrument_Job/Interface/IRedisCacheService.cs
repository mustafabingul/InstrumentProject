using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInstrument_Job.Interface
{
    public interface IRedisCacheService
    {
        Task SetCacheValueAsync(string key, string value);
        Task <string> GetCacheValueAsync(string key);
    }
}
