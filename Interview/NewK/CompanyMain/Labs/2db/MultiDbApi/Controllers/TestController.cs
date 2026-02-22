using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly SqlServerContext _sqlContext;
    private readonly MySqlContext _myContext;

    public TestController(SqlServerContext sqlContext, MySqlContext myContext)
    {
        _sqlContext = sqlContext;
        _myContext = myContext;
    }

    [HttpPost("sql")]
    public async Task<IActionResult> AddSql(string name)
    {
        _sqlContext.Items.Add(new SqlServerItem { Name = name });
        await _sqlContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("sql")]
    public async Task<IActionResult> GetSql()
    {
        return Ok(_sqlContext.Items);
    }

    [HttpPost("mysql")]
    public async Task<IActionResult> AddMySql(string name)
    {
        _myContext.Items.Add(new MySqlItem { Name = name });
        await _myContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("mysql")]
    public async Task<IActionResult> GetMySql()
    {
        return Ok(_myContext.Items);
    }
}