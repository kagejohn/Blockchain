using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Blockchain
{
    class P2PClient
    {
        private readonly IDictionary<string, WebSocket> _webSocketsDictionary = new Dictionary<string, WebSocket>();

        public Blockchain Connect(string url)
        {
            Blockchain newChain = null;

            if (!_webSocketsDictionary.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);
                webSocket.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else
                    {
                        newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                    }
                };
                webSocket.Connect();
                webSocket.Send("Hi Server");
                //ws.Send(JsonConvert.SerializeObject(Program.PhillyCoin));
                _webSocketsDictionary.Add(url, webSocket);
            }

            return newChain;
        }

        public void Send(string url, string data)
        {
            foreach (var item in _webSocketsDictionary)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in _webSocketsDictionary)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in _webSocketsDictionary)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in _webSocketsDictionary)
            {
                item.Value.Close();
            }
        }
    }
}
