using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainInCSharp
{
    public interface IBlockchain
    {
        IList<Block> GetChain();
        Block Create_block(int proof, string previous_hash);
        Block GetPreviousBlock();
        string hash(Block block);
        int ProofOfWork(int previousProof);
        bool IsChainValid(List<Block> chain);
        string CalculateHash(object data);

    }
}
