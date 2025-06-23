using System.Net.WebSockets;
using System.Text;
using ChatWebSocketServer.Services;

namespace ChatWebSocketServer.Handlers;

public class WebSocketHandler
{
    private readonly DialogflowService _dialogflowService;
    private readonly ILogger<WebSocketHandler> _logger;
    public WebSocketHandler(DialogflowService dialogflowService, ILogger<WebSocketHandler> logger)
    {
        _dialogflowService = dialogflowService;
        _logger = logger;
    }
    public async Task HandleAsync(WebSocket socket)
    {
        try
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _logger.LogInformation(message);
                var reply = await _dialogflowService.DetectIntentAsync(message);

                var responseBuffer = Encoding.UTF8.GetBytes(reply);
                await socket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "WebSocket handling failed.");
        }
    }
}
