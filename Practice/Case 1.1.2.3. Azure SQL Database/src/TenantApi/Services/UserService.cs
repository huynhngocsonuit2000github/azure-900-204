using Microsoft.Data.SqlClient;
using System.Data;

namespace TenantApi.Services;

public class UserService
{
    private readonly IConfiguration _config;

    public UserService(IConfiguration config)
    {
        _config = config;
    }

    public async Task CreateTenantDatabaseAsync(string username)
    {
        var dbName = $"TenantDB_{username}";
        var connStr = _config.GetConnectionString("SqlAdmin");

        var createDbSql = $"IF DB_ID('{dbName}') IS NULL CREATE DATABASE [{dbName}];";

        using var conn = new SqlConnection(connStr);
        using var cmd = new SqlCommand(createDbSql, conn);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        // Optional: Create schema/table inside the new DB
        var initTableSql = $@"
            USE [{dbName}];
            IF OBJECT_ID('Products') IS NULL
            CREATE TABLE Products (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(100)
            );";

        cmd.CommandText = initTableSql;
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task TestConnectionAsync()
    {
        var connStr = _config.GetConnectionString("SqlAdmin");

        using var conn = new SqlConnection(connStr);

        await conn.OpenAsync();
    }
}
