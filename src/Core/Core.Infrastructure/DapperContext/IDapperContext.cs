using Core.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.Infrastructure.DapperContext;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}

public class DapperContext : IDapperContext
{
    public IDbConnection CreateConnection() => new SqlConnection(StaticValues.ConnectionString);
}
