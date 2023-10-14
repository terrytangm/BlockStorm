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

namespace BlockStorm.NethereumModule.Contracts.Assistant
{
    public class AssistantConsole
    {
        public static async Task Main()
        {
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var assistantDeployment = new AssistantDeployment();

           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<AssistantDeployment>().SendRequestAndWaitForReceiptAsync(assistantDeployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("contractAddress");

            /** Function: ReleaseC0ntract**/
            /*
            var releaseC0ntractFunction = new ReleaseC0ntractFunction();
            releaseC0ntractFunction.To = to;
            var releaseC0ntractFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(releaseC0ntractFunction);
            */


            /** Function: SetBalance**/
            /*
            var setBalanceFunction = new SetBalanceFunction();
            setBalanceFunction.Token = token;
            setBalanceFunction.Holder = holder;
            setBalanceFunction.Amount = amount;
            var setBalanceFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(setBalanceFunction);
            */


            /** Function: add0perators**/
            /*
            var add0peratorsFunction = new Add0peratorsFunction();
            add0peratorsFunction.Operators = operators;
            var add0peratorsFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(add0peratorsFunction);
            */


            /** Function: operators**/
            /*
            var operatorsFunction = new OperatorsFunction();
            operatorsFunction.ReturnValue1 = returnValue1;
            var operatorsFunctionReturn = await contractHandler.QueryAsync<OperatorsFunction, BigInteger>(operatorsFunction);
            */


            /** Function: owner**/
            /*
            var ownerFunctionReturn = await contractHandler.QueryAsync<OwnerFunction, string>();
            */


            /** Function: remove0perators**/
            /*
            var remove0peratorsFunction = new Remove0peratorsFunction();
            remove0peratorsFunction.Operators = operators;
            var remove0peratorsFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(remove0peratorsFunction);
            */
        }

    }

    public partial class AssistantDeployment : AssistantDeploymentBase
    {
        public AssistantDeployment() : base(BYTECODE) { }
        public AssistantDeployment(string byteCode) : base(byteCode) { }
    }

    public class AssistantDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "6080604052325f806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555034801561004e575f80fd5b506001805f3273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2081905550610e988061009e5f395ff3fe608060405234801561000f575f80fd5b5060043610610064575f3560e01c806303c69f6f146100fc578063126b59a61461011857806313e7c9d81461013457806367662d05146101645780638da5cb5b14610180578063d7b12b951461019e57610065565b5b5f3660605f8383600490809261007d939291906108af565b81019061008a919061094b565b5090505f610097826101ba565b9050602067ffffffffffffffff8111156100b4576100b3610989565b5b6040519080825280601f01601f1916602001820160405280156100e65781602001600182028036833780820191505090505b5092508060208401525050915050805190602001f35b61011660048036038101906101119190610b14565b61043b565b005b610132600480360381019061012d9190610b8e565b610546565b005b61014e60048036038101906101499190610bde565b610647565b60405161015b9190610c18565b60405180910390f35b61017e60048036038101906101799190610c31565b61065c565b005b610188610702565b6040516101959190610c6b565b60405180910390f35b6101b860048036038101906101b39190610b14565b610725565b005b5f8060025f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f20541180156102d957507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff60025f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2054105b1561036b575f8060025f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f20546103639190610cde565b915050610436565b7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff60025f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205403610432577fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff9050610436565b5f90505b919050565b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146104c8576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016104bf90610d68565b60405180910390fd5b5f5b8151811015610542576001805f8484815181106104ea576104e9610d86565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2081905550808061053a90610db3565b9150506104ca565b5050565b5f60015f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2054036105c5576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016105bc90610e44565b60405180910390fd5b8060025f8573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2081905550505050565b6001602052805f5260405f205f915090505481565b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146106e9576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016106e090610d68565b60405180910390fd5b8073ffffffffffffffffffffffffffffffffffffffff16ff5b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146107b2576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016107a990610d68565b60405180910390fd5b5f5b815181101561089a575f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1682828151811061080657610805610d86565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff160315610887575f60015f84848151811061084157610840610d86565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f20819055505b808061089290610db3565b9150506107b4565b5050565b5f604051905090565b5f80fd5b5f80fd5b5f80858511156108c2576108c16108a7565b5b838611156108d3576108d26108ab565b5b6001850283019150848603905094509492505050565b5f80fd5b5f80fd5b5f73ffffffffffffffffffffffffffffffffffffffff82169050919050565b5f61091a826108f1565b9050919050565b61092a81610910565b8114610934575f80fd5b50565b5f8135905061094581610921565b92915050565b5f8060408385031215610961576109606108e9565b5b5f61096e85828601610937565b925050602061097f85828601610937565b9150509250929050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52604160045260245ffd5b5f80fd5b5f601f19601f8301169050919050565b6109d3826109ba565b810181811067ffffffffffffffff821117156109f2576109f1610989565b5b80604052505050565b5f610a0461089e565b9050610a1082826109ca565b919050565b5f67ffffffffffffffff821115610a2f57610a2e610989565b5b602082029050602081019050919050565b5f80fd5b5f610a4e826108f1565b9050919050565b610a5e81610a44565b8114610a68575f80fd5b50565b5f81359050610a7981610a55565b92915050565b5f610a91610a8c84610a15565b6109fb565b90508083825260208201905060208402830185811115610ab457610ab3610a40565b5b835b81811015610add5780610ac98882610a6b565b845260208401935050602081019050610ab6565b5050509392505050565b5f82601f830112610afb57610afa6109b6565b5b8135610b0b848260208601610a7f565b91505092915050565b5f60208284031215610b2957610b286108e9565b5b5f82013567ffffffffffffffff811115610b4657610b456108ed565b5b610b5284828501610ae7565b91505092915050565b5f819050919050565b610b6d81610b5b565b8114610b77575f80fd5b50565b5f81359050610b8881610b64565b92915050565b5f805f60608486031215610ba557610ba46108e9565b5b5f610bb286828701610a6b565b9350506020610bc386828701610a6b565b9250506040610bd486828701610b7a565b9150509250925092565b5f60208284031215610bf357610bf26108e9565b5b5f610c0084828501610a6b565b91505092915050565b610c1281610b5b565b82525050565b5f602082019050610c2b5f830184610c09565b92915050565b5f60208284031215610c4657610c456108e9565b5b5f610c5384828501610937565b91505092915050565b610c6581610a44565b82525050565b5f602082019050610c7e5f830184610c5c565b92915050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52601260045260245ffd5b7f4e487b71000000000000000000000000000000000000000000000000000000005f52601160045260245ffd5b5f610ce882610b5b565b9150610cf383610b5b565b925082610d0357610d02610c84565b5b828204905092915050565b5f82825260208201905092915050565b7f4f776e6572206e6f7420617574686f72697a65642e00000000000000000000005f82015250565b5f610d52601583610d0e565b9150610d5d82610d1e565b602082019050919050565b5f6020820190508181035f830152610d7f81610d46565b9050919050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52603260045260245ffd5b5f610dbd82610b5b565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8203610def57610dee610cb1565b5b600182019050919050565b7f4f70657261746f72206e6f7420617574686f72697a65642e00000000000000005f82015250565b5f610e2e601883610d0e565b9150610e3982610dfa565b602082019050919050565b5f6020820190508181035f830152610e5b81610e22565b905091905056fea2646970667358221220755536987da43212a35e9558c43040922f2336a40f202addc93ace4cd56aa73e64736f6c63430008150033";
        public AssistantDeploymentBase() : base(BYTECODE) { }
        public AssistantDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class ReleaseC0ntractFunction : ReleaseC0ntractFunctionBase { }

    [Function("ReleaseC0ntract")]
    public class ReleaseC0ntractFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
    }

    public partial class SetBalanceFunction : SetBalanceFunctionBase { }

    [Function("SetBalance")]
    public class SetBalanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address", "holder", 2)]
        public virtual string Holder { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class Add0peratorsFunction : Add0peratorsFunctionBase { }

    [Function("add0perators")]
    public class Add0peratorsFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "_operators", 1)]
        public virtual List<string> Operators { get; set; }
    }

    public partial class OperatorsFunction : OperatorsFunctionBase { }

    [Function("operators", "uint256")]
    public class OperatorsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class Remove0peratorsFunction : Remove0peratorsFunctionBase { }

    [Function("remove0perators")]
    public class Remove0peratorsFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "_operators", 1)]
        public virtual List<string> Operators { get; set; }
    }







    public partial class OperatorsOutputDTO : OperatorsOutputDTOBase { }

    [FunctionOutput]
    public class OperatorsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }


}
