using System;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public interface IResponseCacheService
    {
        Task SetCacheAsync(string key, object value, TimeSpan time_out);
        Task<string> GetCacheAsync(string key);

        Task RemoveRedisCacheAsync(string partern);
    }

}
