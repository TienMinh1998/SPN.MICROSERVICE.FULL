using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Hola.Api.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task<string> GetCacheAsync(string key)
        {
            var  cacheResponse = await _distributedCache.GetStringAsync(key);
            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }
        public async Task RemoveRedisCacheAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentException("The Value can not be null or empty!");

            await foreach (var key in GetRedisKeysAsync(pattern + "*"))
            {
                await _distributedCache.RemoveAsync(key);
            }
        }
        private async IAsyncEnumerable<string> GetRedisKeysAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentException("The Value can not be null or empty!");

            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    yield return key.ToString();
                }
            }
        }
        public async Task SetCacheAsync(string key, object value, TimeSpan time_out)
        {
            if (value == null)
                return;

            var serialzerResponse = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(key, serialzerResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = time_out
            });
        }
    }
}
