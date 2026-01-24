using BSDigital.Interfaces;
using System.Net.WebSockets;
using System.Text.Json;
using Websocket.Client;

namespace BSDigital.Clients
{
    public class BitstampClient : IBitstampClient
    {
        private WebsocketClient? _ws;

        public event Action<string>? OnMessage;

        public async Task ConnectAsync()
        {
            _ws = new WebsocketClient(new Uri("wss://ws.bitstamp.net"));
            _ws.MessageReceived.Subscribe(msg =>
            {
                if (!string.IsNullOrEmpty(msg.Text))
                    OnMessage?.Invoke(msg.Text);
            });

            await _ws.Start();

            var msg = new
            {
                @event = "bts:subscribe",
                data = new { channel = "order_book_btceur" }
            };
            _ws.Send(JsonSerializer.Serialize(msg));
        }

        public async Task DisconnectAsync()
        {
            if (_ws is null)
            {
                return;
            }

            await _ws.Stop(WebSocketCloseStatus.NormalClosure, "Disconnecting...");

            _ws.Dispose();
            _ws = null;
        }
    }
}
