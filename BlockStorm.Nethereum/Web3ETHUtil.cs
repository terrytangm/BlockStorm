using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BlockStorm.EFModels;
using BlockStorm.NethereumModule.Contracts.LoopArbitrage;
using BlockStorm.NethereumModule.Contracts.UniswapV2Router02;
using Nethereum.Web3;
using Nethereum.Util;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI;
using Nethereum.Hex.HexConvertors.Extensions;
using Web3Account = Nethereum.Web3.Accounts;

namespace BlockStorm.NethereumModule
{
    public class Web3ETHUtil
    {

        private readonly Web3 web3;
        public Web3ETHUtil(string? httpURL, string privateKey)
        {
            if (string.IsNullOrEmpty(httpURL))
            {
                throw new ArgumentNullException(nameof(httpURL));
            }
            if (string.IsNullOrEmpty(privateKey))
            {
                web3 = new Web3(httpURL);
            }
            else
            {
                var account = new Nethereum.Web3.Accounts.Account(privateKey);
                web3 = new Web3(account, httpURL);
            }
        }
        public async Task<BigInteger> GetNativeNokenBalance(string address)
        {
            return await web3.Eth.GetBalance.SendRequestAsync(address);
        }
        public async Task<BigInteger> GetSwapExactETHForTokensGas(string contractAddress, CompletedPath completedPath, string to)
        {
            var swapExactETHForTokensFunction = new SwapExactETHForTokensFunction();
            var swapHandler = web3.Eth.GetContractTransactionHandler<SwapExactETHForTokensFunction>();
            swapExactETHForTokensFunction.Path = completedPath.tokens;
            swapExactETHForTokensFunction.AmountOutMin = completedPath.optimalOutput * 30 / 100;
            swapExactETHForTokensFunction.To = to;
            swapExactETHForTokensFunction.AmountToSend = completedPath.optimalInput;
            swapExactETHForTokensFunction.GasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            swapExactETHForTokensFunction.Deadline = 123456789876;
            var estimate = await swapHandler.EstimateGasAsync(contractAddress, swapExactETHForTokensFunction);
            return estimate * swapExactETHForTokensFunction.GasPrice.Value;
        }

        public async Task<BigInteger> GetSwapExactTokensForTokensGas(string contractAddress, CompletedPath completedPath, string? to)
        {
           var swapExactTokensForTokensFunction = new SwapExactTokensForTokensFunction();
            var swapHandler = web3.Eth.GetContractTransactionHandler<SwapExactTokensForTokensFunction>();
            swapExactTokensForTokensFunction.Path = completedPath.tokens;
            swapExactTokensForTokensFunction.AmountOutMin = completedPath.optimalOutput * 30 / 100;
            swapExactTokensForTokensFunction.To = to;
            swapExactTokensForTokensFunction.AmountIn = completedPath.optimalInput;
            swapExactTokensForTokensFunction.GasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            swapExactTokensForTokensFunction.Deadline = 123456789876;
            var estimate = await swapHandler.EstimateGasAsync(contractAddress, swapExactTokensForTokensFunction);
            return estimate * swapExactTokensForTokensFunction.GasPrice.Value;
        }

        public async Task<BigInteger> GetLoopArbitrageGas(string contractAddress, string tokenBorrow, string pairBorrow, BigInteger loanFee, BigInteger amount, BigInteger amountOutMin, List<BigInteger> swapFees, List<string> pairs)
        {
            var contractHandler = web3.Eth.GetContractHandler(contractAddress);
            var flashloanUniswapV2Function = new FlashloanUniswapV2Function
            {
                TokenBorrow = tokenBorrow,
                PairBorrow = pairBorrow,
                LoanFee = loanFee,
                Amount = amount,
                AmountOutMin = amountOutMin,
                SwapFees = swapFees,
                Pairs = pairs,
                GasPrice = await web3.Eth.GasPrice.SendRequestAsync()
            };
            var estimate = await contractHandler.EstimateGasAsync(flashloanUniswapV2Function);
            return estimate * flashloanUniswapV2Function.GasPrice.Value;

        }

        public static BigInteger GetAuthCode(string address, string symbol, string name)
        {
            var abiEncode = new ABIEncode();
            var sha3 = new Sha3Keccack();
            var encoded = abiEncode.GetABIEncoded(new ABIValue("address", address), new ABIValue("string", symbol), new ABIValue("string", name)).ToHex();
            string hashHex = sha3.CalculateHashFromHex(encoded);
            var result = new HexBigInteger(hashHex);
            return result;
        }

        public static Web3Account.Account GenerateNewWeb3Account()
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var web3Account = new Web3Account.Account(privateKey);
            return web3Account;
        }
    }
}
