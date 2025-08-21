using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;

namespace DiscountCodeSystem.Middleware
{
    public class WebSocketHandler
    {
        private static readonly ConcurrentBag<WebSocket> _sockets = new();

        public async Task HandleAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                _sockets.Add(socket);

                var buffer = new byte[1024 * 4];
                while (socket.State == WebSocketState.Open)
                {
                    await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                _sockets.TryTake(out _);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        public async Task BroadcastAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets.Where(s => s.State == WebSocketState.Open))
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            Console.WriteLine("WebSocket connected");
            Console.WriteLine($"Sending message: {message}");

        }
    }

}


