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

            /** Function: IsAddressOnFork**/
            /*
            var isAddressOnForkFunction = new IsAddressOnForkFunction();
            isAddressOnForkFunction.ReturnValue1 = returnValue1;
            var isAddressOnForkFunctionReturn = await contractHandler.QueryAsync<IsAddressOnForkFunction, bool>(isAddressOnForkFunction);
            */


            /** Function: ReleaseC0ntract**/
            /*
            var releaseC0ntractFunction = new ReleaseC0ntractFunction();
            releaseC0ntractFunction.To = to;
            var releaseC0ntractFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(releaseC0ntractFunction);
            */


            /** Function: SetAddressOnFork**/
            /*
            var setAddressOnForkFunction = new SetAddressOnForkFunction();
            setAddressOnForkFunction.Addr = addr;
            setAddressOnForkFunction.On = on;
            var setAddressOnForkFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(setAddressOnForkFunction);
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
        public static string BYTECODE = "6080604052326000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555034801561005057600080fd5b5060018060003273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055506110ff806100a46000396000f3fe608060405234801561001057600080fd5b506004361061008c5760003560e01c80638da5cb5b1161005b5780638da5cb5b146101ab578063969bd30a146101c9578063d7b12b95146101f9578063f0f625f6146102155761008d565b806303c69f6f14610127578063126b59a61461014357806313e7c9d81461015f57806367662d051461018f5761008d565b5b60003660606000838360049080926100a793929190610a3e565b8101906100b49190610ae1565b50905060006100c282610231565b9050602067ffffffffffffffff8111156100df576100de610b21565b5b6040519080825280601f01601f1916602001820160405280156101115781602001600182028036833780820191505090505b5092508060208401525050915050805190602001f35b610141600480360381019061013c9190610cb8565b6104d6565b005b61015d60048036038101906101589190610d37565b6105e5565b005b61017960048036038101906101749190610d8a565b6106ed565b6040516101869190610dc6565b60405180910390f35b6101a960048036038101906101a49190610de1565b610705565b005b6101b36107ac565b6040516101c09190610e1d565b60405180910390f35b6101e360048036038101906101de9190610d8a565b6107d0565b6040516101f09190610e53565b60405180910390f35b610213600480360381019061020e9190610cb8565b6107f0565b005b61022f600480360381019061022a9190610e9a565b61096f565b005b600080600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205411801561035957507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054105b801561036a57506103686109ca565b155b1561040157600080600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020546103f99190610f38565b9150506104d1565b7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054036104cc577fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff90506104d1565b600090505b919050565b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614610564576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161055b90610fc6565b60405180910390fd5b60005b81518110156105e157600180600084848151811061058857610587610fe6565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555080806105d990611015565b915050610567565b5050565b6000600160003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205403610667576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161065e906110a9565b60405180910390fd5b80600260008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002081905550505050565b60016020528060005260406000206000915090505481565b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614610793576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161078a90610fc6565b60405180910390fd5b8073ffffffffffffffffffffffffffffffffffffffff16ff5b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b60036020528060005260406000206000915054906101000a900460ff1681565b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff161461087e576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161087590610fc6565b60405180910390fd5b60005b815181101561096b5760008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168282815181106108d4576108d3610fe6565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1603156109585760006001600084848151811061091157610910610fe6565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055505b808061096390611015565b915050610881565b5050565b80600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060006101000a81548160ff0219169083151502179055505050565b6000600360004173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff1680610a25575060014614155b905090565b6000604051905090565b600080fd5b600080fd5b60008085851115610a5257610a51610a34565b5b83861115610a6357610a62610a39565b5b6001850283019150848603905094509492505050565b600080fd5b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b6000610aae82610a83565b9050919050565b610abe81610aa3565b8114610ac957600080fd5b50565b600081359050610adb81610ab5565b92915050565b60008060408385031215610af857610af7610a79565b5b6000610b0685828601610acc565b9250506020610b1785828601610acc565b9150509250929050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b600080fd5b6000601f19601f8301169050919050565b610b6f82610b55565b810181811067ffffffffffffffff82111715610b8e57610b8d610b21565b5b80604052505050565b6000610ba1610a2a565b9050610bad8282610b66565b919050565b600067ffffffffffffffff821115610bcd57610bcc610b21565b5b602082029050602081019050919050565b600080fd5b6000610bee82610a83565b9050919050565b610bfe81610be3565b8114610c0957600080fd5b50565b600081359050610c1b81610bf5565b92915050565b6000610c34610c2f84610bb2565b610b97565b90508083825260208201905060208402830185811115610c5757610c56610bde565b5b835b81811015610c805780610c6c8882610c0c565b845260208401935050602081019050610c59565b5050509392505050565b600082601f830112610c9f57610c9e610b50565b5b8135610caf848260208601610c21565b91505092915050565b600060208284031215610cce57610ccd610a79565b5b600082013567ffffffffffffffff811115610cec57610ceb610a7e565b5b610cf884828501610c8a565b91505092915050565b6000819050919050565b610d1481610d01565b8114610d1f57600080fd5b50565b600081359050610d3181610d0b565b92915050565b600080600060608486031215610d5057610d4f610a79565b5b6000610d5e86828701610c0c565b9350506020610d6f86828701610c0c565b9250506040610d8086828701610d22565b9150509250925092565b600060208284031215610da057610d9f610a79565b5b6000610dae84828501610c0c565b91505092915050565b610dc081610d01565b82525050565b6000602082019050610ddb6000830184610db7565b92915050565b600060208284031215610df757610df6610a79565b5b6000610e0584828501610acc565b91505092915050565b610e1781610be3565b82525050565b6000602082019050610e326000830184610e0e565b92915050565b60008115159050919050565b610e4d81610e38565b82525050565b6000602082019050610e686000830184610e44565b92915050565b610e7781610e38565b8114610e8257600080fd5b50565b600081359050610e9481610e6e565b92915050565b60008060408385031215610eb157610eb0610a79565b5b6000610ebf85828601610c0c565b9250506020610ed085828601610e85565b9150509250929050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601260045260246000fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b6000610f4382610d01565b9150610f4e83610d01565b925082610f5e57610f5d610eda565b5b828204905092915050565b600082825260208201905092915050565b7f4f776e6572206e6f7420617574686f72697a65642e0000000000000000000000600082015250565b6000610fb0601583610f69565b9150610fbb82610f7a565b602082019050919050565b60006020820190508181036000830152610fdf81610fa3565b9050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052603260045260246000fd5b600061102082610d01565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff820361105257611051610f09565b5b600182019050919050565b7f4f70657261746f72206e6f7420617574686f72697a65642e0000000000000000600082015250565b6000611093601883610f69565b915061109e8261105d565b602082019050919050565b600060208201905081810360008301526110c281611086565b905091905056fea2646970667358221220c4b0e252b5b6577ac4e38df0f5bfb8180a06d2e4c626e43dafc1c1e435b37e4864736f6c63430008120033";
        public AssistantDeploymentBase() : base(BYTECODE) { }
        public AssistantDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class IsAddressOnForkFunction : IsAddressOnForkFunctionBase { }

    [Function("IsAddressOnFork", "bool")]
    public class IsAddressOnForkFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ReleaseC0ntractFunction : ReleaseC0ntractFunctionBase { }

    [Function("ReleaseC0ntract")]
    public class ReleaseC0ntractFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
    }

    public partial class SetAddressOnForkFunction : SetAddressOnForkFunctionBase { }

    [Function("SetAddressOnFork")]
    public class SetAddressOnForkFunctionBase : FunctionMessage
    {
        [Parameter("address", "addr", 1)]
        public virtual string Addr { get; set; }
        [Parameter("bool", "on", 2)]
        public virtual bool On { get; set; }
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

    public partial class IsAddressOnForkOutputDTO : IsAddressOnForkOutputDTOBase { }

    [FunctionOutput]
    public class IsAddressOnForkOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
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
