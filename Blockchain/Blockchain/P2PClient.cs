using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Blockchain
{
    class P2PClient
    {
        private readonly IDictionary<string, WebSocket> _webSocketsDictionary = new Dictionary<string, WebSocket>();
        private readonly List<Blockchain> _tempBlockchains = new List<Blockchain>();
        private readonly int _difficulty = Program.Difficulty;

        internal void ConnetAllPeers(List<Peer> peers)
        {
            List<int> blockchainMatchsInt = new List<int>();

            foreach (Peer peer in peers)
            {
                Blockchain blockchain = Connect(peer.Ip, true);

                if (blockchain.IsValid(_difficulty))
                {
                    _tempBlockchains.Add(blockchain);
                }
            }

            foreach (Blockchain blockchain1 in _tempBlockchains)
            {
                List<bool> blockchainMatchBool = new List<bool>();

                foreach (Blockchain blockchain2 in _tempBlockchains)
                {
                    blockchainMatchBool.Add(blockchain1 == blockchain2);
                }

                blockchainMatchsInt.Add(blockchainMatchBool.Count(c => c));
            }

            Program.Blockchain = _tempBlockchains[blockchainMatchsInt.IndexOf(blockchainMatchsInt.Max())];
        }

        public Blockchain Connect(string url, bool initialization = false)
        {
            Blockchain newChain = null;

            if (!_webSocketsDictionary.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);
                webSocket.OnMessage += (sender, e) =>
                {
                    try
                    {
                        newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                        if (!initialization && newChain.IsValid(_difficulty) && newChain.Chain.Count > Program.Blockchain.Chain.Count)
                        {
                            Program.Blockchain = newChain;
                        }
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            Program.Peers.AddRange(JsonConvert.DeserializeObject<List<Peer>>(e.Data));
                        }
                        catch (Exception e1)
                        {
                            Console.WriteLine(e1.Message);
                        }
                    }
                };
                webSocket.Connect();
                webSocket.Send("Tell me about your peers.");
                webSocket.Send(JsonConvert.SerializeObject(Program.Blockchain));
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
