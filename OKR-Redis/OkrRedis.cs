using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OKR_Redis.Interface;
using System.Text.Json;
using OKR_Redis.Redis;
using System.Collections.Generic;
using OKR_Redis.Common;

namespace OKR_Redis
{
    public class OkrRedis
    {
        private static ICacheService _redisCacheService;
        private static ILogger _logger;
        private static string responseMessage;

        [FunctionName("AddtoCache")]
        public static async Task<IActionResult> AddToCache(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            _logger = log;
            _logger.LogInformation("Started: DeleteCache");

            try
            {
                string key = req.Query["key"];

                if (string.IsNullOrEmpty(key))
                    throw new InvalidOperationException("Key can not be empty!");

                _redisCacheService = new RedisCacheService(_logger);

                if (_redisCacheService.Exists(key))
                    throw new InvalidOperationException("Duplicate Key detected!");

                await _redisCacheService.Save(key, key);

                BuildSuccess("Key added successfully");
            }
            catch (Exception ex)
            {
                BuildError(ex.Message);
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("DeleteCache")]
        public static async Task<IActionResult> DeleteCache(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            _logger = log;
            _logger.LogInformation("Started: DeleteCache");

            try
            {
                string key = req.Query["key"];

                if (string.IsNullOrEmpty(key))
                    BuildError("Key can not be empty!");

                _redisCacheService = new RedisCacheService(_logger);

                if (!_redisCacheService.Exists(key))
                    throw new InvalidOperationException("Given Key was not found!");

                await _redisCacheService.Delete(key);

                BuildSuccess("Deleted successfully");
            }
            catch (Exception ex)
            {
                BuildError(ex.Message);
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("FlushCache")]
        public static async Task<IActionResult> FlushCache(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            _logger = log;
            _logger.LogInformation("Started: FlushCache");

            try
            {
                _redisCacheService = new RedisCacheService(_logger);

                await _redisCacheService.Flush();

                BuildSuccess("Cache flushed successfully");
            }
            catch (Exception ex)
            {
                BuildError(ex.Message);
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetCache")]
        public static async Task<IActionResult> GetCache(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            _logger = log;
            _logger.LogInformation("Started: GetCache");
            KeysData keys = new();

            try
            {
                _redisCacheService = new RedisCacheService(_logger);
                keys = await _redisCacheService.GetAll();
            }
            catch (Exception ex)
            {
                BuildError(ex.Message);
            }

            return new OkObjectResult(keys);
        }

        private static void BuildError(string message)
        {
            _logger.LogError(message);
            responseMessage = $"{{\"Result\":\"Error\", \"message\": \"{message}\"}}";
        }

        private static void BuildSuccess(string message)
        {
            _logger.LogInformation(message);
            responseMessage = $"{{\"Result\":\"Success\", \"message\": \"{message}\"}}";
        }
    }
}
