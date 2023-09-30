using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.EFModels
{
    public enum PairDirection
    {
        Token0InToken1Out,
        Token1InToken0Out
    }

    public partial class Pair
    {
        public static FilteredPair ToFilteredPair(Pair pair)
        {
            var filteredPair = new FilteredPair
            {
                PairAddress = pair.PairAddress,
                Token0 = pair.Token0,
                Token1 = pair.Token1,
                Reserve0 = pair.Reserve0,
                Reserve1 = pair.Reserve1,
                Fee = pair.Fee,
                ChainId = pair.ChainId,
                BlockTimeLast = pair.BlockTimeLast,
                Created = pair.Created,
                DexName = pair.DexName,
                LastUpdate = pair.LastUpdate,
                PairIndex = pair.PairIndex,
                Token0In = pair.Token0In,
                Token1In = pair.Token1In
            };
            return filteredPair;
        }

        public static PairDirection GetPairDirection(Pair pair, string tokenIn) 
        {
            if(pair == null)
            {
                throw new ArgumentNullException(nameof(pair));
            }
            if(string.IsNullOrEmpty(tokenIn))
            {
                throw new ArgumentNullException(nameof(tokenIn), tokenIn);
            }
            if (!pair.Token0.Equals(tokenIn) && !pair.Token1.Equals(tokenIn))
            {
                throw new ArgumentException("GetPairDirection: tokenIn matches neither of the tokens of the given pair.");
            }
            return pair.Token0.Equals(tokenIn) ? PairDirection.Token0InToken1Out : PairDirection.Token1InToken0Out;
        }
    }
}
