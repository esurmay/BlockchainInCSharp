using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainInCSharp
{    public class Block
    {
        public int Index { get; set; }
        public string? Timestamp { get; set; }
        public int Proof { get; set; }
        public string? Previous_hash { get; set; }
    }
}
