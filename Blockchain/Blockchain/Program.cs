using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Blockchain
{
    class Program
    {
        private static Blockchain _blockchain;

        static void Main(string[] args)
        {
            _blockchain = new Blockchain();

            Block block = new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}", 0, 4);

            _blockchain.AddBlock(block);

            Console.WriteLine(JsonConvert.SerializeObject(_blockchain, Formatting.Indented));
        }
    }
}
