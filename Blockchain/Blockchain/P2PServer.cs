using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Blockchain
{
    class P2PServer : WebSocketBehavior
    {
        private bool _chainSynched = false;
        private WebSocketServer _webSocketServer = null;
        private readonly int _difficulty = Program.Difficulty;

        public void Start()
        {
            string url = "ws://127.0.0.1:2222";
            _webSocketServer = new WebSocketServer(url);
            _webSocketServer.AddWebSocketService<P2PServer>("/Blockchain");
            _webSocketServer.Start();
            Console.WriteLine($"Started server at {url}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Tell me about your peers.")
            {
                Send(JsonConvert.SerializeObject(Program.Peers));
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid(_difficulty) && newChain.Chain.Count > Program.Blockchain.Chain.Count)
                {
                    Program.Blockchain = newChain;
                }

                if (!_chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.Blockchain));
                    _chainSynched = true;
                }
            }
        }
    }
}
