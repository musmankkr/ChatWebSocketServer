using ChatWebSocketServer.Configurations;
using ChatWebSocketServer.Handlers;
using ChatWebSocketServer.Services;

var builder = WebApplication.CreateBuilder(args);

//Register Services
builder.Services.AddSingleton<DialogflowService>();
builder.Services.AddTransient<WebSocketHandler>();

var config = builder.Configuration;

builder.Services.Configure<DialogflowSettings>(builder.Configuration.GetSection("Dialogflow"));


var app = builder.Build();

app.UseWebSockets();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var handler = context.RequestServices.GetRequiredService<WebSocketHandler>();
        await handler.HandleAsync(webSocket);

    }
    else
    {
        await next(context);
    }
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.Run();