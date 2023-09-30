using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.Utils
{
    public enum AccountType
    {
        evm,
        bitcoin
    }

    public class Util
    {
        public static string GetAmountDisplayWithDecimals(BigInteger input, short decimals)
        {
            if (input.IsZero || decimals == 0) { return input.ToString().Trim(); }
            string strToProcess;
            if (input < 0)
            {
                strToProcess = input.ToString().Trim().Substring(1);
            }
            else
            {
                strToProcess = input.ToString().Trim();
            }
            StringBuilder sb = new(strToProcess);
            if (sb.Length <= decimals)
            {
                sb.Insert(0, "0", decimals - sb.Length + 1);
            }
            sb.Insert(sb.Length - decimals, ".");
            if (input < 0)
            {
                sb.Insert(0, "-");
            }
            return sb.ToString();
        }
        public static BigInteger GetAmountOutThroughSwap(BigInteger amountIn, BigInteger reserveIn, BigInteger reserveOut, short? fee)
        {
            BigInteger amountInWithFee = (BigInteger)(amountIn * (10000 - fee));
            BigInteger numerator = amountInWithFee * reserveOut;
            BigInteger denominator = 10000 * reserveIn + amountInWithFee;
            return numerator / denominator;
        }

        public static void ListRandom<T>(List<T> sources)
        {
            var rd = new Random();
            T temp;
            for (int i = 0; i < sources.Count; i++)
            {
                int index = rd.Next(0, sources.Count - 1);
                if (index != i)
                {
                    temp = sources[i];
                    sources[i] = sources[index];
                    sources[index] = temp;
                }
            }
        }
    }
}
