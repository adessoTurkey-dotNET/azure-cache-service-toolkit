using Microsoft.Extensions.Logging;
using OKR_Redis.Common;
using OKR_Redis.Interface;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OKR_Redis.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _cache;
        private static readonly ConnectionMultiplexer _connectionMultiplexer;
        private static ILogger _logger;

        static RedisCacheService()
        {
            var connection = ConfigurationManager.Instance.CacheConnection;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connection);
        }

        public RedisCacheService(ILogger logger)
        {
            _logger = logger;
            _logger.LogInformation("Calling _connectionMultiplexer.GetDatabase");
            _cache = _connectionMultiplexer.GetDatabase();
            _logger.LogInformation("Called _connectionMultiplexer.GetDatabase");
        }

        public bool Exists(string key)
        {
            _logger.LogInformation($"Started Exists with Key: {key}");
            return _cache.KeyExists(key);
        }

        public async Task Save(string key, string value)
        {
            _logger.LogInformation($"Started Save with Key: {key} - Value: {value}");
            await _cache.StringSetAsync(key, value);
            _logger.LogInformation("Ended Save");
        }

        public async Task<string> Get(string key)
        {
            _logger.LogInformation($"Started Get with Key: {key}");
            return await _cache.StringGetAsync(key);
        }

        public async Task<KeysData> GetAll()
        {
            _logger.LogInformation($"Started GetAll");

            List<KeyData> list = new();
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                IEnumerable<RedisKey> keys = server.Keys(pattern: "*");

                foreach (var key in keys)
                {
                    list.Add(new KeyData()
                    {
                        Key = key,
                        Value = await _cache.StringGetAsync(key)
                    });
                }
            }
            KeysData result = new()
            {
                Keys = list
            };
            return result;
        }

        public async Task Delete(string key)
        {
            _logger.LogInformation($"Started Delete with Key: {key}");
           await _cache.KeyDeleteAsync(key);
            _logger.LogInformation("Ended Delete");
        }

        public async Task Flush()
        {
            _logger.LogInformation("Started Flush");
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
               await server.FlushAllDatabasesAsync();
            }
            _logger.LogInformation("Ended Flush");
        }
    }
}