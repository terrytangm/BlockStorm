using BlockStorm.EFModels;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BlockStorm.Arb.Algorithm
{


    public class FilterPairs
    {
        public static void GetPaths(IEnumerable<FilteredPair> pairs, string tokenIn, string tokenOut, int maxHop, List<FilteredPair> currentPairs, List<string> currentTokenPath, List<CompletedPath> completedPaths)
        {
            if (maxHop == 0) return;
            if (pairs.IsNullOrEmpty<FilteredPair>()) return;
            var pairsFiltered = pairs.Where(p => p.Token0.Equals(tokenIn, StringComparison.OrdinalIgnoreCase) || p.Token1.Equals(tokenIn, StringComparison.OrdinalIgnoreCase));
            if (pairsFiltered.IsNullOrEmpty()) return;
            foreach (FilteredPair pair in pairsFiltered)
            {
                if (!currentPairs.IsNullOrEmpty<FilteredPair>() && currentPairs.Contains(pair)) continue;

                List<FilteredPair> pairsBranch = currentPairs.IsNullOrEmpty<FilteredPair>() ? new() : new(currentPairs);
                pairsBranch.Add(pair);

                string tempOut = pair.Token0.Equals(tokenIn, StringComparison.OrdinalIgnoreCase) ? pair.Token1 : pair.Token0;
                List<string> tokenPathBranch = currentTokenPath.IsNullOrEmpty<string>() ? new() { tokenIn } : new(currentTokenPath);
                tokenPathBranch.Add(tempOut);
                if (tempOut.Equals(tokenOut, StringComparison.OrdinalIgnoreCase))
                {
                    CompletedPath path = new()
                    {
                        pairs = pairsBranch,
                        tokens = tokenPathBranch,
                    };
                    (path.Ea, path.Eb) = GetEaEb(pairsBranch, tokenPathBranch);
                    (path.optimalInput, path.optimalOutput, path.optimalProfit) = GetOptimalInput(path.Ea, path.Eb, 30);
                    completedPaths.Add(path);
                    return;
                }
                else
                {
                    GetPaths(pairs, tempOut, tokenOut, maxHop - 1, pairsBranch, tokenPathBranch, completedPaths);
                }
            }
        }

        public static (BigInteger Ea, BigInteger Eb) GetEaEb(List<FilteredPair> pairPath, List<string> tokenPath)
        {
            if (pairPath.IsNullOrEmpty<FilteredPair>() || tokenPath.IsNullOrEmpty<string>())
            {
                throw new InvalidDataException("pairPath and TokenPath cannot be Null.");
            }
            if (pairPath.Count != tokenPath.Count - 1)
            {
                throw new InvalidDataException("pairPath and TokenPath's element count don't match");
            }
            BigInteger r0, r1;
            if (tokenPath[0].Equals(pairPath[0].Token0, StringComparison.OrdinalIgnoreCase) &&
                tokenPath[1].Equals(pairPath[0].Token1, StringComparison.OrdinalIgnoreCase))
            {
                (r0, r1) = (BigInteger.Parse(pairPath[0].Reserve0), BigInteger.Parse(pairPath[0].Reserve1));
            }
            else if (tokenPath[0].Equals(pairPath[0].Token1, StringComparison.OrdinalIgnoreCase) && 
                       tokenPath[1].Equals(pairPath[0].Token0, StringComparison.OrdinalIgnoreCase))
            {
                (r0, r1) = (BigInteger.Parse(pairPath[0].Reserve1), BigInteger.Parse(pairPath[0].Reserve0));
            }
            else
            {
                throw new InvalidDataException("The tokens on the pairPath don't match with tokenPath");
            }

            for (int i = 1; i < pairPath.Count; i++)
            {
                BigInteger r_1, r2;
                if (tokenPath[i].Equals(pairPath[i].Token0, StringComparison.OrdinalIgnoreCase) &&
                    tokenPath[i + 1].Equals(pairPath[i].Token1, StringComparison.OrdinalIgnoreCase))
                {
                    (r_1, r2) = (BigInteger.Parse(pairPath[i].Reserve0.Trim()), BigInteger.Parse(pairPath[i].Reserve1.Trim()));
                }
                else if (tokenPath[i].Equals(pairPath[i].Token1, StringComparison.OrdinalIgnoreCase) &&
                          tokenPath[i + 1].Equals(pairPath[i].Token0, StringComparison.OrdinalIgnoreCase))
                {
                    (r_1, r2) = (BigInteger.Parse(pairPath[i].Reserve1.Trim()), BigInteger.Parse(pairPath[i].Reserve0.Trim()));
                }
                else
                {
                    throw new InvalidDataException("The tokens on the pairPath don't match with tokenPath");
                }
                BigInteger r0Numerator = 10000 * r0 * r_1;
                BigInteger Denominator = 10000 * r_1 + r1 * (10000 - pairPath[i].Fee.Value);
                if(Denominator.IsZero) return (BigInteger.Zero,BigInteger.Zero);
                r0 = r0Numerator / Denominator;
                BigInteger r1Numerator = (10000 - pairPath[i].Fee.Value) * r1 * r2;
                r1 = r1Numerator / Denominator;
            }
            return (r0, r1);
        }

        public static (BigInteger optimalInput, BigInteger optimalOutput, BigInteger optimalProfit) GetOptimalInput(BigInteger Ea, BigInteger Eb, short fee)
        {
            BigInteger square = Ea * Eb * 10000 / (10000 - fee);
            double sqrt = Math.Exp(BigInteger.Log(square) / 2);
            BigInteger sqrtInt = (BigInteger)sqrt;
            BigInteger optimalInput = sqrtInt - 10000 * Ea / (10000 - fee);

            BigInteger optimalOutputNumerator = (10000 - fee) * Eb * optimalInput;
            BigInteger optimalOutPutDenominator = 10000 * Ea + (10000 - fee) * optimalInput;
            if(optimalOutPutDenominator<=0)
            {
                return (optimalInput, 0,0);
            }
            BigInteger optimalOutput = optimalOutputNumerator / optimalOutPutDenominator;
            BigInteger optimalProfit = optimalOutput - optimalInput;
            return (optimalInput, optimalOutput, optimalProfit);
        }
    }
}