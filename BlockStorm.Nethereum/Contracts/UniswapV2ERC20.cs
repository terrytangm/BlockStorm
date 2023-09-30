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

namespace BlockStorm.NethereumModule.Contracts.UniswapV2ERC20
{


    public class UniswapV2ERC20Console
    {
        public static async Task MainFunc()
        {
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var uniswapV2ERC20Deployment = new UniswapV2ERC20Deployment();

           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<UniswapV2ERC20Deployment>().SendRequestAndWaitForReceiptAsync(uniswapV2ERC20Deployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("contractAddress");

            /** Function: DOMAIN_SEPARATOR**/
            /*
            var domainSeparatorFunctionReturn = await contractHandler.QueryAsync<DomainSeparatorFunction, byte[]>();
            */


            /** Function: PERMIT_TYPEHASH**/
            /*
            var permitTypehashFunctionReturn = await contractHandler.QueryAsync<PermitTypehashFunction, byte[]>();
            */


            /** Function: allowance**/
            /*
            var allowanceFunction = new AllowanceFunction();
            allowanceFunction.ReturnValue1 = returnValue1;
            allowanceFunction.ReturnValue2 = returnValue2;
            var allowanceFunctionReturn = await contractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction);
            */


            /** Function: approve**/
            /*
            var approveFunction = new ApproveFunction();
            approveFunction.Spender = spender;
            approveFunction.Value = value;
            var approveFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(approveFunction);
            */


            /** Function: balanceOf**/
            /*
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.ReturnValue1 = returnValue1;
            var balanceOfFunctionReturn = await contractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction);
            */


            /** Function: decimals**/
            /*
            var decimalsFunctionReturn = await contractHandler.QueryAsync<DecimalsFunction, byte>();
            */


            /** Function: name**/
            /*
            var nameFunctionReturn = await contractHandler.QueryAsync<NameFunction, string>();
            */


            /** Function: nonces**/
            /*
            var noncesFunction = new NoncesFunction();
            noncesFunction.ReturnValue1 = returnValue1;
            var noncesFunctionReturn = await contractHandler.QueryAsync<NoncesFunction, BigInteger>(noncesFunction);
            */


            /** Function: permit**/
            /*
            var permitFunction = new PermitFunction();
            permitFunction.Owner = owner;
            permitFunction.Spender = spender;
            permitFunction.Value = value;
            permitFunction.Deadline = deadline;
            permitFunction.V = v;
            permitFunction.R = r;
            permitFunction.S = s;
            var permitFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(permitFunction);
            */


            /** Function: symbol**/
            /*
            var symbolFunctionReturn = await contractHandler.QueryAsync<SymbolFunction, string>();
            */


            /** Function: totalSupply**/
            /*
            var totalSupplyFunctionReturn = await contractHandler.QueryAsync<TotalSupplyFunction, BigInteger>();
            */


            /** Function: transfer**/
            /*
            var transferFunction = new TransferFunction();
            transferFunction.To = to;
            transferFunction.Value = value;
            var transferFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(transferFunction);
            */


            /** Function: transferFrom**/
            /*
            var transferFromFunction = new TransferFromFunction();
            transferFromFunction.From = from;
            transferFromFunction.To = to;
            transferFromFunction.Value = value;
            var transferFromFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction);
            */
        }

    }

    public partial class UniswapV2ERC20Deployment : UniswapV2ERC20DeploymentBase
    {
        public UniswapV2ERC20Deployment() : base(BYTECODE) { }
        public UniswapV2ERC20Deployment(string byteCode) : base(byteCode) { }
    }

    public class UniswapV2ERC20DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b506000469050604051808061109060529139605201905060405180910390206040518060400160405280600a81526020017f556e697377617020563200000000000000000000000000000000000000000000815250805190602001206040518060400160405280600181526020017f3100000000000000000000000000000000000000000000000000000000000000815250805190602001208330604051602001808681526020018581526020018481526020018381526020018273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001955050505050506040516020818303038152906040528051906020012060038190555050610f618061012f6000396000f3fe608060405234801561001057600080fd5b50600436106100cf5760003560e01c80633644e5151161008c57806395d89b411161006657806395d89b4114610371578063a9059cbb146103f4578063d505accf1461045a578063dd62ed3e146104f3576100cf565b80633644e515146102a357806370a08231146102c15780637ecebe0014610319576100cf565b806306fdde03146100d4578063095ea7b31461015757806318160ddd146101bd57806323b872dd146101db57806330adf81f14610261578063313ce5671461027f575b600080fd5b6100dc61056b565b6040518080602001828103825283818151815260200191508051906020019080838360005b8381101561011c578082015181840152602081019050610101565b50505050905090810190601f1680156101495780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b6101a36004803603604081101561016d57600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803590602001909291905050506105a4565b604051808215151515815260200191505060405180910390f35b6101c56105bb565b6040518082815260200191505060405180910390f35b610247600480360360608110156101f157600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803590602001909291905050506105c1565b604051808215151515815260200191505060405180910390f35b61026961078c565b6040518082815260200191505060405180910390f35b6102876107b3565b604051808260ff1660ff16815260200191505060405180910390f35b6102ab6107b8565b6040518082815260200191505060405180910390f35b610303600480360360208110156102d757600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff1690602001909291905050506107be565b6040518082815260200191505060405180910390f35b61035b6004803603602081101561032f57600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff1690602001909291905050506107d6565b6040518082815260200191505060405180910390f35b6103796107ee565b6040518080602001828103825283818151815260200191508051906020019080838360005b838110156103b957808201518184015260208101905061039e565b50505050905090810190601f1680156103e65780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b6104406004803603604081101561040a57600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff16906020019092919080359060200190929190505050610827565b604051808215151515815260200191505060405180910390f35b6104f1600480360360e081101561047057600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803573ffffffffffffffffffffffffffffffffffffffff1690602001909291908035906020019092919080359060200190929190803560ff169060200190929190803590602001909291908035906020019092919050505061083e565b005b6105556004803603604081101561050957600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803573ffffffffffffffffffffffffffffffffffffffff169060200190929190505050610b82565b6040518082815260200191505060405180910390f35b6040518060400160405280600a81526020017f556e69737761702056320000000000000000000000000000000000000000000081525081565b60006105b1338484610ba7565b6001905092915050565b60005481565b60007fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff600260008673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205414610776576106f582600260008773ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054610c9290919063ffffffff16565b600260008673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055505b610781848484610d15565b600190509392505050565b7f6e71edae12b1b97f4d1f60370fef10105fa2faae0126114a169c64845d6126c960001b81565b601281565b60035481565b60016020528060005260406000206000915090505481565b60046020528060005260406000206000915090505481565b6040518060400160405280600681526020017f554e492d5632000000000000000000000000000000000000000000000000000081525081565b6000610834338484610d15565b6001905092915050565b428410156108b4576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260128152602001807f556e697377617056323a2045585049524544000000000000000000000000000081525060200191505060405180910390fd5b60006003547f6e71edae12b1b97f4d1f60370fef10105fa2faae0126114a169c64845d6126c960001b898989600460008e73ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000815480929190600101919050558a604051602001808781526020018673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020018573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200184815260200183815260200182815260200196505050505050506040516020818303038152906040528051906020012060405160200180807f190100000000000000000000000000000000000000000000000000000000000081525060020183815260200182815260200192505050604051602081830303815290604052805190602001209050600060018286868660405160008152602001604052604051808581526020018460ff1660ff1681526020018381526020018281526020019450505050506020604051602081039080840390855afa158015610a86573d6000803e3d6000fd5b505050602060405103519050600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614158015610afa57508873ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff16145b610b6c576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252601c8152602001807f556e697377617056323a20494e56414c49445f5349474e41545552450000000081525060200191505060405180910390fd5b610b77898989610ba7565b505050505050505050565b6002602052816000526040600020602052806000526040600020600091509150505481565b80600260008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055508173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925836040518082815260200191505060405180910390a3505050565b6000828284039150811115610d0f576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260158152602001807f64732d6d6174682d7375622d756e646572666c6f77000000000000000000000081525060200191505060405180910390fd5b92915050565b610d6781600160008673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054610c9290919063ffffffff16565b600160008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002081905550610dfc81600160008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054610ea990919063ffffffff16565b600160008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055508173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef836040518082815260200191505060405180910390a3505050565b6000828284019150811015610f26576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260148152602001807f64732d6d6174682d6164642d6f766572666c6f7700000000000000000000000081525060200191505060405180910390fd5b9291505056fea265627a7a723158205a4c0aee16201b6e637472e087997cfeb82d8326e24837026fb161bc64aa68aa64736f6c63430005100032454950373132446f6d61696e28737472696e67206e616d652c737472696e672076657273696f6e2c75696e7432353620636861696e49642c6164647265737320766572696679696e67436f6e747261637429";
        public UniswapV2ERC20DeploymentBase() : base(BYTECODE) { }
        public UniswapV2ERC20DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class DomainSeparatorFunction : DomainSeparatorFunctionBase { }

    [Function("DOMAIN_SEPARATOR", "bytes32")]
    public class DomainSeparatorFunctionBase : FunctionMessage
    {

    }

    public partial class PermitTypehashFunction : PermitTypehashFunctionBase { }

    [Function("PERMIT_TYPEHASH", "bytes32")]
    public class PermitTypehashFunctionBase : FunctionMessage
    {

    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("address", "", 2)]
        public virtual string ReturnValue2 { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string HolderAddress { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class NoncesFunction : NoncesFunctionBase { }

    [Function("nonces", "uint256")]
    public class NoncesFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class PermitFunction : PermitFunctionBase { }

    [Function("permit")]
    public class PermitFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3)]
        public virtual BigInteger Value { get; set; }
        [Parameter("uint256", "deadline", 4)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("uint8", "v", 5)]
        public virtual byte V { get; set; }
        [Parameter("bytes32", "r", 6)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "s", 7)]
        public virtual byte[] S { get; set; }
    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true)]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2, true)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3, false)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true)]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3, false)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class DomainSeparatorOutputDTO : DomainSeparatorOutputDTOBase { }

    [FunctionOutput]
    public class DomainSeparatorOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class PermitTypehashOutputDTO : PermitTypehashOutputDTOBase { }

    [FunctionOutput]
    public class PermitTypehashOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint8", "", 1)]
        public virtual byte ReturnValue1 { get; set; }
    }

    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class NoncesOutputDTO : NoncesOutputDTOBase { }

    [FunctionOutput]
    public class NoncesOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
