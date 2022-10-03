﻿using Core.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Cache;

public static class CachingServiceCollectionExtensions
{
    public static IServiceCollection AddCaches(this IServiceCollection services, CachingOptions options = null)
    {
        //services.AddMemoryCache(opt =>
        //{
        //    opt.SizeLimit = options?.InMemory?.SizeLimit;
        //});

        var distributedProvider = options?.Distributed?.Provider;

        if (distributedProvider == "InMemory")
        {
            services.AddDistributedMemoryCache(opt =>
            {
                opt.SizeLimit = options?.Distributed?.InMemory?.SizeLimit;
            });
        }
        else if (distributedProvider == "Redis")
        {
            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = options.Distributed.Redis.Configuration;
                opt.InstanceName = options.Distributed.Redis.InstanceName;
            });
        }
        else if (distributedProvider == "SqlServer")
        {
            services.AddDistributedSqlServerCache(opt =>
            {
                opt.ConnectionString = options.Distributed.SqlServer.ConnectionString;
                opt.SchemaName = options.Distributed.SqlServer.SchemaName;
                opt.TableName = options.Distributed.SqlServer.TableName;
            });
        }
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}