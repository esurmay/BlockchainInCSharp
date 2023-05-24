using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace BlockchainInCSharp
{
   
    public class Blockchain : IBlockchain
    {
        public IList<Block> chain { get; set; }

        public Blockchain()
        {
            if (chain == null)
            {
                chain = new List<Block>();
                Create_block(1, "0"); 
            }
        }

        public IList<Block> GetChain() 
        {
            return chain;
        }

        public Block Create_block(int proof, string previous_hash)
        {

            //chain[proof] = previous_hash;

            var block = new Block
            {
                Index = chain.Count + 1,
                Timestamp = ConvertDateTimeToTimestamp(DateTime.UtcNow).ToString(),
                Previous_hash = previous_hash,
                Proof = proof
            };

            chain.Add(block);
            return block;

        }
        public Block GetPreviousBlock()
        {                 
            return chain[chain.Count - 1];
        }
        public string hash(Block block)
        {
            string encodedBlock = JsonConvert.SerializeObject(block, Newtonsoft.Json.Formatting.None, 
                new JsonSerializerSettings { 
                    NullValueHandling = NullValueHandling.Ignore, 
                    StringEscapeHandling = StringEscapeHandling.Default, 
                    Formatting = Newtonsoft.Json.Formatting.None, 
                    DefaultValueHandling = DefaultValueHandling.Ignore 
                });

            byte[] encodedBytes = Encoding.UTF8.GetBytes(encodedBlock);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(encodedBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        private static double ConvertDateTimeToTimestamp(DateTime value)
        {
            TimeSpan epoch = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return the total seconds (which is a UNIX timestamp)
            return (double)epoch.TotalSeconds;
        }

        public int ProofOfWork(int previousProof)
        {
            int newProof = 1;
            bool checkProof = false;

            while (!checkProof)
            {
                string hashOperation = CalculateHash((newProof * newProof) - (previousProof * previousProof));
                if (hashOperation.Substring(0, 4) == "0000")
                    checkProof = true;
                else
                    newProof++;
            }

            return newProof;
        }

        public bool IsChainValid(List<Block> chain)
        {
            Block previousBlock = chain[0];
            int blockIndex = 1;

            while (blockIndex < chain.Count)
            {
                Block block = chain[blockIndex];

                if (block.Previous_hash != CalculateHash(previousBlock))
                    return false;

                int previousProof = previousBlock.Proof;
                int proof = block.Proof;

                string hashOperation = CalculateHash((proof * proof) - (previousProof * previousProof));
                if (hashOperation.Substring(0, 4) != "0000")
                    return false;

                previousBlock = block;
                blockIndex++;
            }

            return true;
        }

        public string CalculateHash(object data)
        {
            string jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None);
            byte[] encodedData = Encoding.UTF8.GetBytes(jsonData);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(encodedData);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    builder.Append(hashBytes[i].ToString("x2"));

                return builder.ToString();
            }
        }
    }

}