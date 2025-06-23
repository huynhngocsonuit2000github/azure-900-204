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
        Console.WriteLine("CW - Request come " + DateTime.Now.ToString());
        _logger.LogInformation("@@@Logger Information - Request come " + DateTime.Now.ToString());
        _logger.LogWarning("@@@Logger Warning - Request come " + DateTime.Now.ToString());

        return Ok("Hello world!");
    }

    [HttpGet("cpu")]
    public IActionResult GetCPU()
    {
        var m = DateTime.Now.ToString();
        Console.WriteLine("CW - cpu come " + m);
        var ls = new List<long>();
        for (int i = 0; i < 1000; i++)
        {
            var a = i * i * i;
            ls.Add(a);
        }
        Console.WriteLine("CW - cpu end " + m);

        return Ok("Hello world! CPU");
    }
}
