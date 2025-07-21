using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SimpleAuthApi.Models;
using SimpleAuthApi.Static;

namespace SimpleAuthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] User login)
    {
        var user = UserStore.Users.FirstOrDefault(u =>
            u.Username == login.Username && u.Password == login.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        // For demo: return user info (no token here)
        return Ok(new { user.Username, user.FullName });
    }

    [HttpGet("login-azure")]
    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(
            new AuthenticationProperties
            {
                RedirectUri = "http://localhost:4200"
            },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("custom-login-handler")]
    public IActionResult CustomLoginHandler()
    {
        // if (!HttpContext.User.Identity?.IsAuthenticated ?? true)
        // {
        //     return Unauthorized("User not authenticated");
        // }

        // Set a custom cookie
        Response.Cookies.Append("custom-token", "value", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });
        Response.Cookies.Append("sos", "kakkkkak", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Redirect("http://localhost:4200/profile");
    }


    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult Me()
    {
        var user = HttpContext.User;

        var userInfo = new
        {
            Name = user.Identity?.Name,
            IsAuthenticated = user.Identity?.IsAuthenticated,
            Claims = user.Claims.Select(c => new { c.Type, c.Value })
        };

        return Ok(userInfo);
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetDataFromBAsync()
    {

        // 1. App A credentials
        // mess

        // 2. Create confidential client
        var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
            .Build();

        // 3. Acquire token
        var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        string token = result.AccessToken;

        // 4. Call API B
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync("https://localhost:5001/api/data");
        string content = await response.Content.ReadAsStringAsync();

        return Ok(content);
    }
}