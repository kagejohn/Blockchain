using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blockchain
{
    //Google search: "C# blockchain example"
    //https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/
    class Blockchain
    {
        public IList<Block> Chain { set; get; }

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }


        public void InitializeChain()
        {
            Chain = new List<Block>();
        }

        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, "{}", 0, 1, "sequential");
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            Chain.Add(block);
        }

        public bool IsValid(int difficulty)
        {
            string prefix = "";

            for (int i = 0; i < difficulty; i++)
            {
                prefix += "0";
            }

            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }

                if (!currentBlock.Hash.StartsWith(prefix))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
