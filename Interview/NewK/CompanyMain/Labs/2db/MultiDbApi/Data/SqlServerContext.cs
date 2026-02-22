using Microsoft.EntityFrameworkCore;

public class SqlServerContext : DbContext
{
    public SqlServerContext(DbContextOptions<SqlServerContext> options)
        : base(options) { }

    public DbSet<SqlServerItem> Items => Set<SqlServerItem>();
}