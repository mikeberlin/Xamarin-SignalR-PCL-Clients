using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRChatRoom.Core
{
    public class Client
    {
        private readonly string _platform;
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;

        public event EventHandler<string> OnMessageReceived;

        public Client(string platform)
        {
            _platform = platform;
			_connection = new HubConnection("http://mikes-sandbox.azurewebsites.net");
            _proxy = _connection.CreateHubProxy("Chat");
        }

        public async Task Connect()
        {
            await _connection.Start();

            _proxy.On("messageReceived", (string platform, string message) =>
            {
                if (OnMessageReceived != null)
                    OnMessageReceived(this, string.Format("{0}: {1}", platform, message));
            });

            await Send("Connected");
        }

        public Task Send(string message)
        {
            return _proxy.Invoke("Send", _platform, message);
        }
    }
}