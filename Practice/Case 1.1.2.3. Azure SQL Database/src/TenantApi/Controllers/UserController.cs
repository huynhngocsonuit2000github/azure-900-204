using Microsoft.AspNetCore.Mvc;
using TenantApi.Models;
using TenantApi.Services;

namespace TenantApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly DatabaseRouter _router;

    public UserController(UserService userService, DatabaseRouter router)
    {
        _userService = userService;
        _router = router;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        await _userService.TestConnectionAsync();
        return Ok($"✅ Connection successful");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRequest request)
    {
        await _userService.CreateTenantDatabaseAsync(request.Username);
        return Ok($"✅ Created DB for user: {request.Username}");
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct(UserRequest request)
    {
        await _router.InsertProductAsync(request.Username, request.ProductName);
        return Ok($"✅ Added product for user: {request.Username}");
    }
}
