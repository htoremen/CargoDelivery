using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Interfaces;

public interface ICacheService
{
    T Get<T>(string cacheKey);
    void Set<T>(string cacheKey, T value);
    void Remove(string cacheKey);
}