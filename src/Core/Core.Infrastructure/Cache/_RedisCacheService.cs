//using Core.Application.Common.Interfaces;
//using Microsoft.Extensions.Caching.Distributed;
//using Newtonsoft.Json;
//using System.Text;

//namespace Core.Infrastructure.Cache;

//public class RedisCacheService : ICacheService
//{
//    private IDistributedCache _cache;

//    public RedisCacheService(IDistributedCache cache)
//    {
//        _cache = cache;
//    }
//    public async Task<T> GetAsync<T>(string cacheKey)
//    {
//        var data = await _cache.GetAsync(cacheKey);
//        var cachedMessage = Encoding.UTF8.GetString(data);
//        var value = JsonConvert.DeserializeObject<T>(cachedMessage);
//        return value;
//    }

//    public async Task SetAsync<T>(string cacheKey, T value)
//    {
//        var dataSerialize = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
//        {
//            PreserveReferencesHandling = PreserveReferencesHandling.Objects
//        });
//        await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(dataSerialize));
//    }

//    public async Task RemoveAsync(string cacheKey)
//    {
//        await _cache.RemoveAsync(cacheKey);
//    }
//}