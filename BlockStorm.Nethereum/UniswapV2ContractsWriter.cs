using BlockStorm.NethereumModule.Contracts.UniswapV2ERC20;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.NethereumModule.Contracts.UniswapDelegate;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.NethereumModule
{
    public class UniswapV2ContractsWriter
    {
        private readonly Web3 web3;
        public UniswapV2ContractsWriter(string httpUrl, string privateKey)
        {
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            web3 = new Web3(account, httpUrl);
        }

        public async Task Transfer(string tokenAddress, string to, BigInteger amount)
        {
            var contractHandler = web3.Eth.GetContractHandler(tokenAddress);
            var transferFunction = new Contracts.UniswapV2ERC20.TransferFunction
            {
                To = to,
                Value = amount
            };
            await contractHandler.SendRequestAndWaitForReceiptAsync(transferFunction);
        }

        public async Task Swap(string pairAddress, BigInteger amount0Out, BigInteger amount1Out, string to, byte[] data)
        {
            var contractHandler = web3.Eth.GetContractHandler(pairAddress);
            var swapFunction = new Contracts.UniswapV2Pair.SwapFunction
            {
                Amount0Out = amount0Out,
                Amount1Out = amount1Out,
                To = to,
                Data = data
            };
            await contractHandler.SendRequestAndWaitForReceiptAsync(swapFunction);
        }

        public async Task SwapThroughDelegate(string delegateAddress, string pairAddress, BigInteger amount0Out, BigInteger amount1Out, string to, byte[] data)
        {
            var contractHandler = web3.Eth.GetContractHandler(delegateAddress);
            var swapFunction = new Contracts.UniswapDelegate.SwapFunction
            {
                Pair = pairAddress,
                Amount0Out = amount0Out,
                Amount1Out = amount1Out,
                To = to,
                Data = data
            };
            await contractHandler.SendRequestAndWaitForReceiptAsync(swapFunction);
        }
    }
}
