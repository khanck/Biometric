using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
namespace TCC.Biometric.Payment.Handlers
{
    public class WebSocketHandler
    {
        // Use ConcurrentDictionary to store connected clients
        private static ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();

        public async Task HandleWebSocketConnection(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                //var clientId = context.Request?.QueryString.Value?.Split("=")[1];
                if (!context.Request.Query.TryGetValue("clientId", out var clientId))
                {
                 
                        clientId = Guid.NewGuid().ToString();
                    
                }
               

                // Add the new client to the dictionary
                _clients.TryAdd(clientId, webSocket);

                Console.WriteLine($"Client connected: {clientId}");

                // Handle incoming messages from the client (if necessary)
                await ReceiveMessagesAsync(webSocket, clientId);

                // Remove client when they disconnect
                _clients.TryRemove(clientId, out _);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task ReceiveMessagesAsync(WebSocket webSocket, string clientId)
        {
            var buffer = new byte[1024 * 4];

            WebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message from client {clientId}: {message}");
               await SendMessageToClient( "100",message);
                // Process the message if needed

            } while (!result.CloseStatus.HasValue);

            // Close the connection when done
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            Console.WriteLine($"Client disconnected: {clientId}");
        }

        public async Task SendMessageToClient(string clientId, string message)
        {
           if (_clients.TryGetValue(clientId, out var webSocket) && webSocket.State == WebSocketState.Open)
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
           }
        }

        public async Task BroadcastMessage(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients)
            {
                var webSocket = client.Value;
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
