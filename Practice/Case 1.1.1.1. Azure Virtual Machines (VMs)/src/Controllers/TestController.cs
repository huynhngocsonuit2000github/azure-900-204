using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    public IActionResult Get()
    {
        _logger.LogWarning("Jenkins Pro Warning - CPU - Request come " + DateTime.Now.ToString());

        return Ok("Hello world son!");
    }

    [HttpGet("cpu")]
    public IActionResult GetCPU()
    {
        _logger.LogWarning("Jenkins Pro Warning - CPU - Request come " + DateTime.Now.ToString());

        var ls = new List<long>();
        for (int i = 0; i < 1000; i++)
        {
            var a = i * i * i;
            ls.Add(a);
        }

        return Ok("Hello world! son CPU");
    }
}
