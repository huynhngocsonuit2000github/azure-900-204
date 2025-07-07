using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

class Program
{
    private const string connectionString = 
    "Server=tcp:sql-server-test-2288.database.windows.net,1433;Initial Catalog=my-sql-database-test-123;Persist Security Info=False;User ID=useradmin;Password=SOn01698182219;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Connecting to Azure SQL...");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                await conn.OpenAsync();
                Console.WriteLine("Connected!");

                string sql = "SELECT Id, Name FROM Products";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        Console.WriteLine($"Product: {id} - {name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
