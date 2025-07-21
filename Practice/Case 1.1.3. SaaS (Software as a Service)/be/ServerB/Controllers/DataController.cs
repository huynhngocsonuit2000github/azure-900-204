using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerB.Controllers;

[ApiController]
[Route("api/data")]
public class DataController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        System.Console.WriteLine("Call this method for Application Authentication level");
        return Ok(new List<object>()
        {
            new { id= 1, title= "Item 1" },
            new { id= 2, title= "Item 2" }
        });
    }
}
