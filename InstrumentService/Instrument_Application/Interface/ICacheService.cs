using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Application.Interface
{
    public interface ICacheService
    {
        Task<string> GetCacheValue(string key);
       
    }
}
