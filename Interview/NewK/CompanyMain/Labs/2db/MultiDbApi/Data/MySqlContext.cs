using Microsoft.EntityFrameworkCore;

public class MySqlContext : DbContext
{
    public MySqlContext(DbContextOptions<MySqlContext> options)
        : base(options) { }

    public DbSet<MySqlItem> Items => Set<MySqlItem>();
}