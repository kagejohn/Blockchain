using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Blockchain
{
    class Program
    {
        internal static Blockchain Blockchain;
        internal static readonly List<Peer> Peers = new List<Peer>();
        internal static readonly int Difficulty = 4;
        private static readonly P2PClient Client = new P2PClient();

        static void Main(string[] args)
        {
            DefaultPeers();

            DiscoverPeers();

            GetBlockchainFromPeers();

            if (Blockchain == null)
            {
                Blockchain = new Blockchain();
            }

            while (true)
            {
                Miner();

                Console.WriteLine(JsonConvert.SerializeObject(Blockchain, Formatting.Indented));// For test
            }
        }

        private static void Miner()
        {
            string minerType = "";

            Random random = new Random();
            switch (random.Next(1, 2))
            {
                case 1:
                    minerType = "sequential";
                    break;
                case 2:
                    minerType = "random";
                    break;
            }

            Block block = new Block(DateTime.Now, null, "{Hello World!}", 0, Difficulty, minerType);

            Blockchain.AddBlock(block);

            Client.Broadcast(JsonConvert.SerializeObject(Blockchain));
        }

        private static void DiscoverPeers()
        {
            P2PServer server = new P2PServer();
            server.Start();
        }

        private static void DefaultPeers()
        {
            List<Peer> knownPeers = new List<Peer>();// Add 3-5 here

            knownPeers.Add(new Peer {Ip = "127.0.0.1", LastSignOfLife = DateTime.Now});
            knownPeers.Add(new Peer {Ip = "127.0.0.1", LastSignOfLife = DateTime.Now});
            knownPeers.Add(new Peer {Ip = "127.0.0.1", LastSignOfLife = DateTime.Now});

            Random random = new Random();
            int firstRandom = random.Next(0, knownPeers.Count - 1);
            Peers.Add(knownPeers[firstRandom]);

            int secondRandom = firstRandom;
            while (secondRandom == firstRandom)
            {
                secondRandom = random.Next(0, knownPeers.Count - 1);
            }

            Peers.Add(knownPeers[secondRandom]);
        }

        private static void GetBlockchainFromPeers()
        {
            Client.ConnetAllPeers(Peers);
        }
    }
}