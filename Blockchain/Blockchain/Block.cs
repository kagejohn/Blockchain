﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    //Google search: "C# blockchain example"
    //https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/
    class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }
        public int Nonce { get; set; }

        public Block(DateTime timeStamp, string previousHash, string data, int nonce, int difficulty, string minerType)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Nonce = nonce;
            switch (minerType.ToLower())
            {
                case "sequential":
                    Hash = MineBlockSequential(difficulty);
                    break;
                case "random":
                    Hash = MineBlockRandom(difficulty);
                    break;
            }
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}-{Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public string MineBlockSequential(int difficulty)
        {
            string hash = "";

            DateTime dateTime = DateTime.Now;

            string prefix = "";

            for (int i = 0; i < difficulty; i++)
            {
                prefix += "0";
            }

            while (!hash.StartsWith(prefix))
            {
                hash = CalculateHash();

                if (DateTime.Now > dateTime + TimeSpan.FromSeconds(1))
                {
                    Console.WriteLine("Nonce: " + Nonce.ToString("N0"));
                    dateTime = DateTime.Now;
                }

                Nonce++;
            }

            return hash;
        }

        public string MineBlockRandom(int difficulty)
        {
            int tries = 1;

            if (Nonce == 0)
            {
                Nonce = 1;
            }

            string hash = "";

            DateTime dateTime = DateTime.Now;

            string prefix = "";

            for (int i = 0; i < difficulty; i++)
            {
                prefix += "0";
            }

            Random random = new Random();
            HashSet<int> hashSetInts = new HashSet<int>();

            while (!hash.StartsWith(prefix))
            {
                hash = CalculateHash();

                if (DateTime.Now > dateTime + TimeSpan.FromSeconds(1))
                {
                    Console.WriteLine("Tries: " + tries.ToString("N0"));
                    dateTime = DateTime.Now;
                }


                Nonce = Convert.ToInt32(int.MaxValue * random.NextDouble());

                while (hashSetInts.Contains(Nonce))
                {
                    Nonce++;
                }

                hashSetInts.Add(Nonce);
                tries++;
            }

            return hash;
        }
    }
}
