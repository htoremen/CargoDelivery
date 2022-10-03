using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure;

public class CachingOptions
{
    public DistributedCacheOptions Distributed { get; set; }
}

public class DistributedCacheOptions
{
    public string Provider { get; set; }

    public InMemoryCacheOptions InMemory { get; set; }

    public RedisOptions Redis { get; set; }

    public SqlServerOptions SqlServer { get; set; }
}

public class RedisOptions
{
    public string Configuration { get; set; }

    public string InstanceName { get; set; }
}

public class SqlServerOptions
{
    public string ConnectionString { get; set; }

    public string SchemaName { get; set; }

    public string TableName { get; set; }
}

public class InMemoryCacheOptions
{
    public long? SizeLimit { get; set; }
}
