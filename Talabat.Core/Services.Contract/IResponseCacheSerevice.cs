using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
    public interface IResponseCacheSerevice
    {
        Task CacheResponseAsync(string key, object Response, TimeSpan timeTolive);

     
        Task<string?> GetCacheResponseAsync(string key);

    }
}
