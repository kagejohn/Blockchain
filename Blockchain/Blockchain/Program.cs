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

        static void Main(string[] args)
        {
            DefaultPeers();

            DiscoverPeers();

            GetBlockchainFromPeers();

            Blockchain = new Blockchain();// For test

            Miner();

            Console.WriteLine(JsonConvert.SerializeObject(Blockchain, Formatting.Indented));// For test
        }

        private static void Miner()
        {
            string minerType = "";

            Random random = new Random();
            random.Next(1, 2);
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
        }

        private static void DiscoverPeers()
        {
            
        }

        private static void DefaultPeers()
        {
            Peers.Add(new Peer {Ip = "", LastSignOfLife = DateTime.Now});// Fill this
        }

        private static void GetBlockchainFromPeers()
        {
            P2PClient client = new P2PClient();
            client.ConnetAllPeers(Peers);
        }
    }
}