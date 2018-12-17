using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Blockchain
{
    class Program
    {
        public static Blockchain Blockchain;
        private static readonly List<Peer> Peers = new List<Peer>();
        private static readonly int Difficulty = 4;
        private static readonly List<Blockchain> TempBlockchains = new List<Blockchain>();

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
            List<int> blockchainMatchsInt = new List<int>();

            foreach (Peer peer in Peers)
            {
                P2PClient peerClient = new P2PClient();
                Blockchain blockchain = peerClient.Connect(peer.Ip);

                if (blockchain.IsValid(Difficulty))
                {
                    TempBlockchains.Add(blockchain);
                }
            }

            foreach (Blockchain blockchain1 in TempBlockchains)
            {
                List<bool> blockchainMatchBool = new List<bool>();

                foreach (Blockchain blockchain2 in TempBlockchains)
                {
                    blockchainMatchBool.Add(blockchain1 == blockchain2);
                }

                blockchainMatchsInt.Add(blockchainMatchBool.Count(c => c));
            }

            Blockchain = TempBlockchains[blockchainMatchsInt.IndexOf(blockchainMatchsInt.Max())];
        }
    }
}