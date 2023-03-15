using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Dapper.Contexts;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection") ?? throw new ArgumentNullException("Empty connection string");
    }

    public DapperContext()
    { }

    public virtual IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
