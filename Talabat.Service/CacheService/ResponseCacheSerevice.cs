using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.CacheService
{
    public class ResponseCacheSerevice : IResponseCacheSerevice
    {
        private readonly IDatabase _database;

        public ResponseCacheSerevice(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        public async Task CacheResponseAsync(string key, object Response, TimeSpan timeTolive)
        {
            if (Response is null) return;

            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serializedResponse = JsonSerializer.Serialize(Response, serializeOptions);

            await _database.StringSetAsync(key, serializedResponse, timeTolive);
        }

        public async Task<string?> GetCacheResponseAsync(string key)
        {
            var response = await _database.StringGetAsync(key); 

            if (response.IsNullOrEmpty) return null;

            return response;
        }
    }
}
