using Microsoft.Data.SqlClient;

namespace TenantApi.Services;

public class DatabaseRouter
{
    private readonly IConfiguration _config;

    public DatabaseRouter(IConfiguration config)
    {
        _config = config;
    }

    public async Task InsertProductAsync(string username, string productName)
    {
        var dbName = $"TenantDB_{username}";
        var baseConn = _config.GetConnectionString("SqlAdmin");

        var connStr = new SqlConnectionStringBuilder(baseConn)
        {
            InitialCatalog = dbName
        }.ToString();

        using var conn = new SqlConnection(connStr);
        var insertSql = "INSERT INTO Products (Name) VALUES (@Name)";
        using var cmd = new SqlCommand(insertSql, conn);

        cmd.Parameters.AddWithValue("@Name", productName);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
