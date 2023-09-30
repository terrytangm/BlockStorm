using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.EFModels
{
    public class CompletedPath
    {
        public List<FilteredPair>? pairs;
        public List<string>? tokens;
        public BigInteger Ea;
        public BigInteger Eb;
        public BigInteger optimalInput;
        public BigInteger optimalOutput;
        public BigInteger optimalProfit;
    }
}
