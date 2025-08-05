using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<ChatHub>("/chat");

app.Run("http://localhost:5000");


public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine($"Received from {user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }


    public async Task PlusTwoNumber(string user, int num1, int num2)
    {
        Console.WriteLine($"Request to plus two numbers: {num1}, {num2}");
        await Clients.All.SendAsync("TotalMessage", user, $"{num1} + {num2} = {num1 + num2}");
    }
}