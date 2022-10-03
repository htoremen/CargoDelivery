using Core.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Core.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    private IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    public T Get<T>(string cacheKey)
    {
        var data = _cache.Get(cacheKey);
        var cachedMessage = Encoding.UTF8.GetString(data);
        var value = JsonConvert.DeserializeObject<T>(cachedMessage);
        return value;
    }

    public void Set<T>(string cacheKey, T value)
    {
        var dataSerialize = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
        _cache.Set(cacheKey, Encoding.UTF8.GetBytes(dataSerialize));
    }

    public void Remove(string cacheKey)
    {
        _cache.Remove(cacheKey);
    }
}