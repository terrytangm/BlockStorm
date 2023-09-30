using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace BlockStorm.NethereumModule.Contracts.UniswapDelegate
{
    public class UniswapDelegateConsole
    {
        public static async Task Main()
        {
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var uniswapDelegateDeployment = new UniswapDelegateDeployment();

           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<UniswapDelegateDeployment>().SendRequestAndWaitForReceiptAsync(uniswapDelegateDeployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("");

            /** Function: swap**/
            /*
            var swapFunction = new SwapFunction();
            swapFunction.Pair = pair;
            swapFunction.Amount0Out = amount0Out;
            swapFunction.Amount1Out = amount1Out;
            swapFunction.To = to;
            swapFunction.Data = data;
            var swapFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapFunction);
            */
        }

    }

    public partial class UniswapDelegateDeployment : UniswapDelegateDeploymentBase
    {
        public UniswapDelegateDeployment() : base(BYTECODE) { }
        public UniswapDelegateDeployment(string byteCode) : base(byteCode) { }
    }

    public class UniswapDelegateDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b50610364806100206000396000f3fe608060405234801561001057600080fd5b506004361061002b5760003560e01c806330e8d2c614610030575b600080fd5b61004a600480360381019061004591906101ca565b61004c565b005b8573ffffffffffffffffffffffffffffffffffffffff1663022c0d9f86868686866040518663ffffffff1660e01b815260040161008d9594939291906102e0565b600060405180830381600087803b1580156100a757600080fd5b505af11580156100bb573d6000803e3d6000fd5b50505050505050505050565b600080fd5b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b60006100fc826100d1565b9050919050565b61010c816100f1565b811461011757600080fd5b50565b60008135905061012981610103565b92915050565b6000819050919050565b6101428161012f565b811461014d57600080fd5b50565b60008135905061015f81610139565b92915050565b600080fd5b600080fd5b600080fd5b60008083601f84011261018a57610189610165565b5b8235905067ffffffffffffffff8111156101a7576101a661016a565b5b6020830191508360018202830111156101c3576101c261016f565b5b9250929050565b60008060008060008060a087890312156101e7576101e66100c7565b5b60006101f589828a0161011a565b965050602061020689828a01610150565b955050604061021789828a01610150565b945050606061022889828a0161011a565b935050608087013567ffffffffffffffff811115610249576102486100cc565b5b61025589828a01610174565b92509250509295509295509295565b61026d8161012f565b82525050565b61027c816100f1565b82525050565b600082825260208201905092915050565b82818337600083830152505050565b6000601f19601f8301169050919050565b60006102bf8385610282565b93506102cc838584610293565b6102d5836102a2565b840190509392505050565b60006080820190506102f56000830188610264565b6103026020830187610264565b61030f6040830186610273565b81810360608301526103228184866102b3565b9050969550505050505056fea26469706673582212209fe6ed5c5bd2aabc760dbcaaecc35a3527bc4acd5ab8047335ad1a6e760cd9a164736f6c63430008120033";
        public UniswapDelegateDeploymentBase() : base(BYTECODE) { }
        public UniswapDelegateDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class SwapFunction : SwapFunctionBase { }

    [Function("swap")]
    public class SwapFunctionBase : FunctionMessage
    {
        [Parameter("address", "pair", 1)]
        public virtual string Pair { get; set; }
        [Parameter("uint256", "amount0Out", 2)]
        public virtual BigInteger Amount0Out { get; set; }
        [Parameter("uint256", "amount1Out", 3)]
        public virtual BigInteger Amount1Out { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("bytes", "data", 5)]
        public virtual byte[] Data { get; set; }
    }


}
