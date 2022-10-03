using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Interfaces;

public interface ICacheService
{
    Task<T> GetAsync<T>(string cacheKey);
    Task SetAsync<T>(string cacheKey, T value);
    Task RemoveAsync(string cacheKey);
}