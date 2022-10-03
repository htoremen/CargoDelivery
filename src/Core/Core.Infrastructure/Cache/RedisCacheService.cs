using Core.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    public void Remove(string cacheKey)
    {
        throw new NotImplementedException();
    }
    public T Set<T>(string cacheKey, T value)
    {
        throw new NotImplementedException();
    }
    public bool TryGet<T>(string cacheKey, out T value)
    {
        throw new NotImplementedException();
    }
}