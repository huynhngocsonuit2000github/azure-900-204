using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyHttpFunction;

public class GreetUser
{
    private readonly ILogger<GreetUser> _logger;

    public GreetUser(ILogger<GreetUser> logger)
    {
        _logger = logger;
    }

    [Function("GreetUser")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        return new OkObjectResult("Hi user!");
    }
}
