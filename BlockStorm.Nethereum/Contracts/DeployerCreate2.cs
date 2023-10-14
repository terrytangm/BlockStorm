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

namespace BlockStorm.NethereumModule.Contracts.DeployerCreate2
{
    public class DeployerCreate2Console
    {
        public static async Task Main()
        {
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var deployerCreate2Deployment = new DeployerCreate2Deployment();

           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<DeployerCreate2Deployment>().SendRequestAndWaitForReceiptAsync(deployerCreate2Deployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("contractAddress");

            /** Function: Depl0yContract93258**/
            /*
            var depl0yContract93258Function = new Depl0yContract93258Function();
            depl0yContract93258Function.Amount = amount;
            depl0yContract93258Function.Salt = salt;
            depl0yContract93258Function.Bytecode = bytecode;
            var depl0yContract93258FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(depl0yContract93258Function);
            */


            /** Function: add0perators**/
            /*
            var add0peratorsFunction = new Add0peratorsFunction();
            add0peratorsFunction.Operators = operators;
            var add0peratorsFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(add0peratorsFunction);
            */


            /** Function: computeAddress79306**/
            /*
            var computeAddress79306Function = new ComputeAddress79306Function();
            computeAddress79306Function.Salt = salt;
            computeAddress79306Function.BytecodeHash = bytecodeHash;
            var computeAddress79306FunctionReturn = await contractHandler.QueryAsync<ComputeAddress79306Function, string>(computeAddress79306Function);
            */


            /** Function: computeAddress85178**/
            /*
            var computeAddress85178Function = new ComputeAddress85178Function();
            computeAddress85178Function.Salt = salt;
            computeAddress85178Function.BytecodeHash = bytecodeHash;
            computeAddress85178Function.Deployer = deployer;
            var computeAddress85178FunctionReturn = await contractHandler.QueryAsync<ComputeAddress85178Function, string>(computeAddress85178Function);
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

    public partial class DeployerCreate2Deployment : DeployerCreate2DeploymentBase
    {
        public DeployerCreate2Deployment() : base(BYTECODE) { }
        public DeployerCreate2Deployment(string byteCode) : base(byteCode) { }
    }

    public class DeployerCreate2DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "6080604052325f806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555034801561004e575f80fd5b506001805f3273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2081905550610e4d8061009e5f395ff3fe608060405234801561000f575f80fd5b506004361061007b575f3560e01c806314a0cd481161005957806314a0cd48146100fb5780638da5cb5b1461012b5780639822cb1b14610149578063d7b12b95146101795761007b565b806303c69f6f1461007f578063073677b71461009b57806313e7c9d8146100cb575b5f80fd5b6100996004803603810190610094919061088c565b610195565b005b6100b560048036038101906100b091906109e9565b6102a0565b6040516100c29190610a64565b60405180910390f35b6100e560048036038101906100e09190610a7d565b610334565b6040516100f29190610ab7565b60405180910390f35b61011560048036038101906101109190610ad0565b610349565b6040516101229190610a64565b60405180910390f35b6101336103db565b6040516101409190610a64565b60405180910390f35b610163600480360381019061015e9190610b0e565b6103fe565b6040516101709190610a64565b60405180910390f35b610193600480360381019061018e919061088c565b610413565b005b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614610222576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161021990610bb8565b60405180910390fd5b5f5b815181101561029c576001805f84848151811061024457610243610bd6565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2081905550808061029490610c30565b915050610224565b5050565b5f8060015f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205403610320576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161031790610cc1565b60405180910390fd5b61032b84848461058c565b90509392505050565b6001602052805f5260405f205f915090505481565b5f8060015f3373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f2054036103c9576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016103c090610cc1565b60405180910390fd5b6103d38383610693565b905092915050565b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b5f61040a8484846106a7565b90509392505050565b5f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146104a0576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161049790610bb8565b60405180910390fd5b5f5b8151811015610588575f8054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168282815181106104f4576104f3610bd6565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff160315610575575f60015f84848151811061052f5761052e610bd6565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f20819055505b808061058090610c30565b9150506104a2565b5050565b5f834710156105d0576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016105c790610d29565b60405180910390fd5b5f825103610613576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161060a90610d91565b60405180910390fd5b8282516020840186f590505f73ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff160361068c576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161068390610df9565b60405180910390fd5b9392505050565b5f61069f8383306106a7565b905092915050565b5f604051836040820152846020820152828152600b810160ff815360558120925050509392505050565b5f604051905090565b5f80fd5b5f80fd5b5f80fd5b5f601f19601f8301169050919050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52604160045260245ffd5b61072c826106e6565b810181811067ffffffffffffffff8211171561074b5761074a6106f6565b5b80604052505050565b5f61075d6106d1565b90506107698282610723565b919050565b5f67ffffffffffffffff821115610788576107876106f6565b5b602082029050602081019050919050565b5f80fd5b5f73ffffffffffffffffffffffffffffffffffffffff82169050919050565b5f6107c68261079d565b9050919050565b6107d6816107bc565b81146107e0575f80fd5b50565b5f813590506107f1816107cd565b92915050565b5f6108096108048461076e565b610754565b9050808382526020820190506020840283018581111561082c5761082b610799565b5b835b81811015610855578061084188826107e3565b84526020840193505060208101905061082e565b5050509392505050565b5f82601f830112610873576108726106e2565b5b81356108838482602086016107f7565b91505092915050565b5f602082840312156108a1576108a06106da565b5b5f82013567ffffffffffffffff8111156108be576108bd6106de565b5b6108ca8482850161085f565b91505092915050565b5f819050919050565b6108e5816108d3565b81146108ef575f80fd5b50565b5f81359050610900816108dc565b92915050565b5f819050919050565b61091881610906565b8114610922575f80fd5b50565b5f813590506109338161090f565b92915050565b5f80fd5b5f67ffffffffffffffff821115610957576109566106f6565b5b610960826106e6565b9050602081019050919050565b828183375f83830152505050565b5f61098d6109888461093d565b610754565b9050828152602081018484840111156109a9576109a8610939565b5b6109b484828561096d565b509392505050565b5f82601f8301126109d0576109cf6106e2565b5b81356109e084826020860161097b565b91505092915050565b5f805f60608486031215610a00576109ff6106da565b5b5f610a0d868287016108f2565b9350506020610a1e86828701610925565b925050604084013567ffffffffffffffff811115610a3f57610a3e6106de565b5b610a4b868287016109bc565b9150509250925092565b610a5e816107bc565b82525050565b5f602082019050610a775f830184610a55565b92915050565b5f60208284031215610a9257610a916106da565b5b5f610a9f848285016107e3565b91505092915050565b610ab1816108d3565b82525050565b5f602082019050610aca5f830184610aa8565b92915050565b5f8060408385031215610ae657610ae56106da565b5b5f610af385828601610925565b9250506020610b0485828601610925565b9150509250929050565b5f805f60608486031215610b2557610b246106da565b5b5f610b3286828701610925565b9350506020610b4386828701610925565b9250506040610b54868287016107e3565b9150509250925092565b5f82825260208201905092915050565b7f4f776e6572206e6f7420617574686f72697a65642e00000000000000000000005f82015250565b5f610ba2601583610b5e565b9150610bad82610b6e565b602082019050919050565b5f6020820190508181035f830152610bcf81610b96565b9050919050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52603260045260245ffd5b7f4e487b71000000000000000000000000000000000000000000000000000000005f52601160045260245ffd5b5f610c3a826108d3565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8203610c6c57610c6b610c03565b5b600182019050919050565b7f4f70657261746f72206e6f7420617574686f72697a65642e00000000000000005f82015250565b5f610cab601883610b5e565b9150610cb682610c77565b602082019050919050565b5f6020820190508181035f830152610cd881610c9f565b9050919050565b7f437265617465323a20696e73756666696369656e742062616c616e63650000005f82015250565b5f610d13601d83610b5e565b9150610d1e82610cdf565b602082019050919050565b5f6020820190508181035f830152610d4081610d07565b9050919050565b7f437265617465323a2062797465636f6465206c656e677468206973207a65726f5f82015250565b5f610d7b602083610b5e565b9150610d8682610d47565b602082019050919050565b5f6020820190508181035f830152610da881610d6f565b9050919050565b7f437265617465323a204661696c6564206f6e206465706c6f79000000000000005f82015250565b5f610de3601983610b5e565b9150610dee82610daf565b602082019050919050565b5f6020820190508181035f830152610e1081610dd7565b905091905056fea2646970667358221220674f8d5f389fceb338f8ed2636b3b482d021910a75b4a18667327a20bc53101464736f6c63430008150033";
        public DeployerCreate2DeploymentBase() : base(BYTECODE) { }
        public DeployerCreate2DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class Depl0yContract93258Function : Depl0yContract93258FunctionBase { }

    [Function("Depl0yContract93258", "address")]
    public class Depl0yContract93258FunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes32", "salt", 2)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes", "bytecode", 3)]
        public virtual byte[] Bytecode { get; set; }
    }

    public partial class Add0peratorsFunction : Add0peratorsFunctionBase { }

    [Function("add0perators")]
    public class Add0peratorsFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "_operators", 1)]
        public virtual List<string> Operators { get; set; }
    }

    public partial class ComputeAddress79306Function : ComputeAddress79306FunctionBase { }

    [Function("computeAddress79306", "address")]
    public class ComputeAddress79306FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "bytecodeHash", 2)]
        public virtual byte[] BytecodeHash { get; set; }
    }

    public partial class ComputeAddress85178Function : ComputeAddress85178FunctionBase { }

    [Function("computeAddress85178", "address")]
    public class ComputeAddress85178FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "bytecodeHash", 2)]
        public virtual byte[] BytecodeHash { get; set; }
        [Parameter("address", "deployer", 3)]
        public virtual string Deployer { get; set; }
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





    public partial class ComputeAddress79306OutputDTO : ComputeAddress79306OutputDTOBase { }

    [FunctionOutput]
    public class ComputeAddress79306OutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ComputeAddress85178OutputDTO : ComputeAddress85178OutputDTOBase { }

    [FunctionOutput]
    public class ComputeAddress85178OutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "addr", 1)]
        public virtual string Addr { get; set; }
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
