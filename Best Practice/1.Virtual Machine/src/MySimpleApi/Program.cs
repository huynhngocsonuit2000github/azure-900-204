var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello mấy cưng, deploy version 3!");
app.MapGet("/api/hello/{name}", (string name) => $"Hello, {name}! Welcome to .NET 8 API!");
app.MapGet("/api/user", () => new { Id = 1, Name = "Son", Role = "Admin" });

app.Run();
