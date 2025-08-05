using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/chat")
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});
connection.On<string, string>("TotalMessage", (user, message) =>
{
    Console.WriteLine($"{message}");
});

await connection.StartAsync();
Console.WriteLine("Connected to SignalR Hub!");

while (true)
{
    // Console.Write("You: ");
    // var msg = Console.ReadLine();
    // await connection.InvokeAsync("SendMessage", "ConsoleClient", msg);
    var num1 = Int32.Parse(Console.ReadLine());
    var num2 = Int32.Parse(Console.ReadLine());

    await connection.InvokeAsync("PlusTwoNumber", "ConsoleClient", num1, num2);

}
