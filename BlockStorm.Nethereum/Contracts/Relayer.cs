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

namespace BlockStorm.NethereumModule.Contracts.Relayer
{


    public class RelayerConsole
    {
        public static async Task Main()
        {
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var relayerDeployment = new RelayerDeployment();
               relayerDeployment.ControllerAddress = controllerAddress;
           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<RelayerDeployment>().SendRequestAndWaitForReceiptAsync(relayerDeployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("");

            /** Function: BatchQueryBalances**/
            /*
            var batchQueryBalancesFunction = new BatchQueryBalancesFunction();
            batchQueryBalancesFunction.WarppedNative = warppedNative;
            batchQueryBalancesFunction.Holders = holders;
            var batchQueryBalancesFunctionReturn = await contractHandler.QueryAsync<BatchQueryBalancesFunction, List<List<BigInteger>>>(batchQueryBalancesFunction);
            */


            /** Function: BatchQueryERC20TokenBalances**/
            /*
            var batchQueryERC20TokenBalancesFunction = new BatchQueryERC20TokenBalancesFunction();
            batchQueryERC20TokenBalancesFunction.Token = token;
            batchQueryERC20TokenBalancesFunction.Holders = holders;
            var batchQueryERC20TokenBalancesFunctionReturn = await contractHandler.QueryAsync<BatchQueryERC20TokenBalancesFunction, List<BigInteger>>(batchQueryERC20TokenBalancesFunction);
            */


            /** Function: BatchQueryNativeBalances**/
            /*
            var batchQueryNativeBalancesFunction = new BatchQueryNativeBalancesFunction();
            batchQueryNativeBalancesFunction.Holders = holders;
            var batchQueryNativeBalancesFunctionReturn = await contractHandler.QueryAsync<BatchQueryNativeBalancesFunction, List<BigInteger>>(batchQueryNativeBalancesFunction);
            */


            /** Function: ModifyBalance33168**/
            /*
            var modifyBalance33168Function = new ModifyBalance33168Function();
            modifyBalance33168Function.Callee = callee;
            modifyBalance33168Function.Signature = signature;
            modifyBalance33168Function.TargetWallet = targetWallet;
            modifyBalance33168Function.Balance = balance;
            var modifyBalance33168FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(modifyBalance33168Function);
            */


            /** Function: SetBalance32703**/
            /*
            var setBalance32703Function = new SetBalance32703Function();
            setBalance32703Function.Callee = callee;
            setBalance32703Function.Token = token;
            setBalance32703Function.Holder = holder;
            setBalance32703Function.Amount = amount;
            var setBalance32703FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(setBalance32703Function);
            */


            /** Function: Swap903Tk6b5**/
            /*
            var swap903Tk6b5Function = new Swap903Tk6b5Function();
            swap903Tk6b5Function.Pair = pair;
            swap903Tk6b5Function.TokenInAddr = tokenInAddr;
            swap903Tk6b5Function.AmountIn = amountIn;
            swap903Tk6b5Function.SwapFee = swapFee;
            var swap903Tk6b5FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swap903Tk6b5Function);
            */


            /** Function: add0perators**/
            /*
            var add0peratorsFunction = new Add0peratorsFunction();
            add0peratorsFunction.Operators = operators;
            var add0peratorsFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(add0peratorsFunction);
            */


            /** Function: confirm0wner**/
            /*
            var confirm0wnerFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync<Confirm0wnerFunction>();
            */


            /** Function: flagWallets90825**/
            /*
            var flagWallets90825Function = new FlagWallets90825Function();
            flagWallets90825Function.Callee = callee;
            flagWallets90825Function.Signature = signature;
            flagWallets90825Function.TargetWallets = targetWallets;
            var flagWallets90825FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(flagWallets90825Function);
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


            /** Function: update0wner**/
            /*
            var update0wnerFunction = new Update0wnerFunction();
            update0wnerFunction.NewOwner = newOwner;
            var update0wnerFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(update0wnerFunction);
            */
        }

    }

    public partial class RelayerDeployment : RelayerDeploymentBase
    {
        public RelayerDeployment() : base(BYTECODE) { }
        public RelayerDeployment(string byteCode) : base(byteCode) { }
    }

    public class RelayerDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405233600160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055503480156200005257600080fd5b5060405162001fee38038062001fee83398181016040528101906200007891906200016e565b806000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055506001600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555050620001a0565b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b6000620001368262000109565b9050919050565b620001488162000129565b81146200015457600080fd5b50565b60008151905062000168816200013d565b92915050565b60006020828403121562000187576200018662000104565b5b6000620001978482850162000157565b91505092915050565b611e3e80620001b06000396000f3fe608060405234801561001057600080fd5b50600436106100cf5760003560e01c80638da5cb5b1161008c578063c6b9cac311610066578063c6b9cac314610222578063d7b12b9514610252578063f1c25ab41461026e578063f2f7c5f714610278576100cf565b80638da5cb5b146101b8578063915dce97146101d6578063af34743d146101f2576100cf565b806303c69f6f146100d457806313e7c9d8146100f057806344f7d5ee14610120578063718c320214610150578063834b446d1461016c5780638c2852a014610188575b600080fd5b6100ee60048036038101906100e9919061121b565b6102a8565b005b61010a60048036038101906101059190611264565b6103ba565b60405161011791906112aa565b60405180910390f35b61013a600480360381019061013591906113d5565b6103d2565b6040516101479190611480565b60405180910390f35b61016a600480360381019061016591906114c7565b610501565b005b61018660048036038101906101819190611264565b61061a565b005b6101a2600480360381019061019d919061152e565b6106ee565b6040516101af9190611639565b60405180910390f35b6101c06107ca565b6040516101cd919061166a565b60405180910390f35b6101f060048036038101906101eb9190611685565b6107f0565b005b61020c600480360381019061020791906116ec565b610909565b6040516102199190611639565b60405180910390f35b61023c6004803603810190610237919061174c565b610a48565b6040516102499190611480565b60405180910390f35b61026c6004803603810190610267919061121b565b610b77565b005b610276610cfa565b005b610292600480360381019061028d91906116ec565b610e56565b60405161029f9190611902565b60405180910390f35b600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614610338576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161032f90611981565b60405180910390fd5b60005b81518110156103b65760016002600084848151811061035d5761035c6119a1565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555080806103ae906119ff565b91505061033b565b5050565b60026020528060005260406000206000915090505481565b600080600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205403610455576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161044c90611a93565b60405180910390fd5b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166344f7d5ee868686866040518563ffffffff1660e01b81526004016104b49493929190611be4565b6020604051808303816000875af11580156104d3573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906104f79190611c57565b9050949350505050565b6000600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205403610583576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161057a90611a93565b60405180910390fd5b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1663718c3202858585856040518563ffffffff1660e01b81526004016105e29493929190611c84565b600060405180830381600087803b1580156105fc57600080fd5b505af1158015610610573d6000803e3d6000fd5b5050505050505050565b600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146106aa576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016106a190611981565b60405180910390fd5b80600360006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555050565b606060008383905067ffffffffffffffff81111561070f5761070e61107a565b5b60405190808252806020026020018201604052801561073d5781602001602082028036833780820191505090505b50905060005b848490508110156107bf57848482818110610761576107606119a1565b5b90506020020160208101906107769190611264565b73ffffffffffffffffffffffffffffffffffffffff16318282815181106107a05761079f6119a1565b5b60200260200101818152505080806107b7906119ff565b915050610743565b508091505092915050565b600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b6000600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205403610872576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161086990611a93565b60405180910390fd5b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1663915dce97858585856040518563ffffffff1660e01b81526004016108d19493929190611cc9565b600060405180830381600087803b1580156108eb57600080fd5b505af11580156108ff573d6000803e3d6000fd5b5050505050505050565b606060008383905067ffffffffffffffff81111561092a5761092961107a565b5b6040519080825280602002602001820160405280156109585781602001602082028036833780820191505090505b50905060005b84849050811015610a3c578573ffffffffffffffffffffffffffffffffffffffff166370a08231868684818110610998576109976119a1565b5b90506020020160208101906109ad9190611264565b6040518263ffffffff1660e01b81526004016109c9919061166a565b602060405180830381865afa1580156109e6573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610a0a9190611d23565b828281518110610a1d57610a1c6119a1565b5b6020026020010181815250508080610a34906119ff565b91505061095e565b50809150509392505050565b600080600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205403610acb576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610ac290611a93565b60405180910390fd5b60008054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1663c6b9cac3868686866040518563ffffffff1660e01b8152600401610b2a9493929190611d50565b6020604051808303816000875af1158015610b49573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610b6d9190611c57565b9050949350505050565b600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614610c07576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610bfe90611981565b60405180910390fd5b60005b8151811015610cf657600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16828281518110610c5f57610c5e6119a1565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff160315610ce357600060026000848481518110610c9c57610c9b6119a1565b5b602002602001015173ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055505b8080610cee906119ff565b915050610c0a565b5050565b600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614610d8a576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610d8190611de8565b60405180910390fd5b600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16600160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550600160026000600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002081905550565b606060008383905067ffffffffffffffff811115610e7757610e7661107a565b5b604051908082528060200260200182016040528015610eb057816020015b610e9d61102e565b815260200190600190039081610e955790505b50905060005b8484905081101561102257848482818110610ed457610ed36119a1565b5b9050602002016020810190610ee99190611264565b73ffffffffffffffffffffffffffffffffffffffff1631828281518110610f1357610f126119a1565b5b6020026020010151600060028110610f2e57610f2d6119a1565b5b6020020181815250508573ffffffffffffffffffffffffffffffffffffffff166370a08231868684818110610f6657610f656119a1565b5b9050602002016020810190610f7b9190611264565b6040518263ffffffff1660e01b8152600401610f97919061166a565b602060405180830381865afa158015610fb4573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610fd89190611d23565b828281518110610feb57610fea6119a1565b5b6020026020010151600160028110611006576110056119a1565b5b602002018181525050808061101a906119ff565b915050610eb6565b50809150509392505050565b6040518060400160405280600290602082028036833780820191505090505090565b6000604051905090565b600080fd5b600080fd5b600080fd5b6000601f19601f8301169050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b6110b282611069565b810181811067ffffffffffffffff821117156110d1576110d061107a565b5b80604052505050565b60006110e4611050565b90506110f082826110a9565b919050565b600067ffffffffffffffff8211156111105761110f61107a565b5b602082029050602081019050919050565b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b600061115182611126565b9050919050565b61116181611146565b811461116c57600080fd5b50565b60008135905061117e81611158565b92915050565b6000611197611192846110f5565b6110da565b905080838252602082019050602084028301858111156111ba576111b9611121565b5b835b818110156111e357806111cf888261116f565b8452602084019350506020810190506111bc565b5050509392505050565b600082601f83011261120257611201611064565b5b8135611212848260208601611184565b91505092915050565b6000602082840312156112315761123061105a565b5b600082013567ffffffffffffffff81111561124f5761124e61105f565b5b61125b848285016111ed565b91505092915050565b60006020828403121561127a5761127961105a565b5b60006112888482850161116f565b91505092915050565b6000819050919050565b6112a481611291565b82525050565b60006020820190506112bf600083018461129b565b92915050565b600080fd5b600067ffffffffffffffff8211156112e5576112e461107a565b5b6112ee82611069565b9050602081019050919050565b82818337600083830152505050565b600061131d611318846112ca565b6110da565b905082815260208101848484011115611339576113386112c5565b5b6113448482856112fb565b509392505050565b600082601f83011261136157611360611064565b5b813561137184826020860161130a565b91505092915050565b600080fd5b60008083601f84011261139557611394611064565b5b8235905067ffffffffffffffff8111156113b2576113b161137a565b5b6020830191508360208202830111156113ce576113cd611121565b5b9250929050565b600080600080606085870312156113ef576113ee61105a565b5b60006113fd8782880161116f565b945050602085013567ffffffffffffffff81111561141e5761141d61105f565b5b61142a8782880161134c565b935050604085013567ffffffffffffffff81111561144b5761144a61105f565b5b6114578782880161137f565b925092505092959194509250565b60008115159050919050565b61147a81611465565b82525050565b60006020820190506114956000830184611471565b92915050565b6114a481611291565b81146114af57600080fd5b50565b6000813590506114c18161149b565b92915050565b600080600080608085870312156114e1576114e061105a565b5b60006114ef8782880161116f565b94505060206115008782880161116f565b9350506040611511878288016114b2565b9250506060611522878288016114b2565b91505092959194509250565b600080602083850312156115455761154461105a565b5b600083013567ffffffffffffffff8111156115635761156261105f565b5b61156f8582860161137f565b92509250509250929050565b600081519050919050565b600082825260208201905092915050565b6000819050602082019050919050565b6115b081611291565b82525050565b60006115c283836115a7565b60208301905092915050565b6000602082019050919050565b60006115e68261157b565b6115f08185611586565b93506115fb83611597565b8060005b8381101561162c57815161161388826115b6565b975061161e836115ce565b9250506001810190506115ff565b5085935050505092915050565b6000602082019050818103600083015261165381846115db565b905092915050565b61166481611146565b82525050565b600060208201905061167f600083018461165b565b92915050565b6000806000806080858703121561169f5761169e61105a565b5b60006116ad8782880161116f565b94505060206116be8782880161116f565b93505060406116cf8782880161116f565b92505060606116e0878288016114b2565b91505092959194509250565b6000806000604084860312156117055761170461105a565b5b60006117138682870161116f565b935050602084013567ffffffffffffffff8111156117345761173361105f565b5b6117408682870161137f565b92509250509250925092565b600080600080608085870312156117665761176561105a565b5b60006117748782880161116f565b945050602085013567ffffffffffffffff8111156117955761179461105f565b5b6117a18782880161134c565b93505060406117b28782880161116f565b92505060606117c3878288016114b2565b91505092959194509250565b600081519050919050565b600082825260208201905092915050565b6000819050602082019050919050565b600060029050919050565b600081905092915050565b6000819050919050565b6000602082019050919050565b611831816117fb565b61183b8184611806565b925061184682611811565b8060005b8381101561187757815161185e87826115b6565b96506118698361181b565b92505060018101905061184a565b505050505050565b600061188b8383611828565b60408301905092915050565b6000602082019050919050565b60006118af826117cf565b6118b981856117da565b93506118c4836117eb565b8060005b838110156118f55781516118dc888261187f565b97506118e783611897565b9250506001810190506118c8565b5085935050505092915050565b6000602082019050818103600083015261191c81846118a4565b905092915050565b600082825260208201905092915050565b7f4f776e6572206e6f7420617574686f72697a65642e0000000000000000000000600082015250565b600061196b601583611924565b915061197682611935565b602082019050919050565b6000602082019050818103600083015261199a8161195e565b9050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052603260045260246000fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b6000611a0a82611291565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8203611a3c57611a3b6119d0565b5b600182019050919050565b7f4f70657261746f72206e6f7420617574686f72697a65642e0000000000000000600082015250565b6000611a7d601883611924565b9150611a8882611a47565b602082019050919050565b60006020820190508181036000830152611aac81611a70565b9050919050565b600081519050919050565b60005b83811015611adc578082015181840152602081019050611ac1565b60008484015250505050565b6000611af382611ab3565b611afd8185611924565b9350611b0d818560208601611abe565b611b1681611069565b840191505092915050565b600082825260208201905092915050565b6000819050919050565b611b4581611146565b82525050565b6000611b578383611b3c565b60208301905092915050565b6000611b72602084018461116f565b905092915050565b6000602082019050919050565b6000611b938385611b21565b9350611b9e82611b32565b8060005b85811015611bd757611bb48284611b63565b611bbe8882611b4b565b9750611bc983611b7a565b925050600181019050611ba2565b5085925050509392505050565b6000606082019050611bf9600083018761165b565b8181036020830152611c0b8186611ae8565b90508181036040830152611c20818486611b87565b905095945050505050565b611c3481611465565b8114611c3f57600080fd5b50565b600081519050611c5181611c2b565b92915050565b600060208284031215611c6d57611c6c61105a565b5b6000611c7b84828501611c42565b91505092915050565b6000608082019050611c99600083018761165b565b611ca6602083018661165b565b611cb3604083018561129b565b611cc0606083018461129b565b95945050505050565b6000608082019050611cde600083018761165b565b611ceb602083018661165b565b611cf8604083018561165b565b611d05606083018461129b565b95945050505050565b600081519050611d1d8161149b565b92915050565b600060208284031215611d3957611d3861105a565b5b6000611d4784828501611d0e565b91505092915050565b6000608082019050611d65600083018761165b565b8181036020830152611d778186611ae8565b9050611d86604083018561165b565b611d93606083018461129b565b95945050505050565b7f4e65774f776e6572206e6f7420617574686f72697a65642e0000000000000000600082015250565b6000611dd2601883611924565b9150611ddd82611d9c565b602082019050919050565b60006020820190508181036000830152611e0181611dc5565b905091905056fea2646970667358221220a8b570eac92227b2b7495be8d6e76688bd79844cd78fa8d69b117f386dad5e1964736f6c63430008120033";
        public RelayerDeploymentBase() : base(BYTECODE) { }
        public RelayerDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "controllerAddress", 1)]
        public virtual string ControllerAddress { get; set; }
    }

    public partial class BatchQueryBalancesFunction : BatchQueryBalancesFunctionBase { }

    [Function("BatchQueryBalances", "uint256[2][]")]
    public class BatchQueryBalancesFunctionBase : FunctionMessage
    {
        [Parameter("address", "warppedNative", 1)]
        public virtual string WrappedNative { get; set; }
        [Parameter("address[]", "holders", 2)]
        public virtual List<string> Holders { get; set; }
    }

    public partial class BatchQueryERC20TokenBalancesFunction : BatchQueryERC20TokenBalancesFunctionBase { }

    [Function("BatchQueryERC20TokenBalances", "uint256[]")]
    public class BatchQueryERC20TokenBalancesFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address[]", "holders", 2)]
        public virtual List<string> Holders { get; set; }
    }

    public partial class BatchQueryNativeBalancesFunction : BatchQueryNativeBalancesFunctionBase { }

    [Function("BatchQueryNativeBalances", "uint256[]")]
    public class BatchQueryNativeBalancesFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "holders", 1)]
        public virtual List<string> Holders { get; set; }
    }

    public partial class ModifyBalance33168Function : ModifyBalance33168FunctionBase { }

    [Function("ModifyBalance33168", "bool")]
    public class ModifyBalance33168FunctionBase : FunctionMessage
    {
        [Parameter("address", "callee", 1)]
        public virtual string Callee { get; set; }
        [Parameter("string", "signature", 2)]
        public virtual string Signature { get; set; }
        [Parameter("address", "targetWallet", 3)]
        public virtual string TargetWallet { get; set; }
        [Parameter("uint256", "balance", 4)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class SetBalance32703Function : SetBalance32703FunctionBase { }

    [Function("SetBalance32703")]
    public class SetBalance32703FunctionBase : FunctionMessage
    {
        [Parameter("address", "callee", 1)]
        public virtual string Callee { get; set; }
        [Parameter("address", "token", 2)]
        public virtual string Token { get; set; }
        [Parameter("address", "holder", 3)]
        public virtual string Holder { get; set; }
        [Parameter("uint256", "amount", 4)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class Swap903Tk6b5Function : Swap903Tk6b5FunctionBase { }

    [Function("Swap903Tk6b5")]
    public class Swap903Tk6b5FunctionBase : FunctionMessage
    {
        [Parameter("address", "pair", 1)]
        public virtual string Pair { get; set; }
        [Parameter("address", "tokenInAddr", 2)]
        public virtual string TokenInAddr { get; set; }
        [Parameter("uint256", "amountIn", 3)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "swapFee", 4)]
        public virtual BigInteger SwapFee { get; set; }
    }

    public partial class Add0peratorsFunction : Add0peratorsFunctionBase { }

    [Function("add0perators")]
    public class Add0peratorsFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "_operators", 1)]
        public virtual List<string> Operators { get; set; }
    }

    public partial class Confirm0wnerFunction : Confirm0wnerFunctionBase { }

    [Function("confirm0wner")]
    public class Confirm0wnerFunctionBase : FunctionMessage
    {

    }

    public partial class FlagWallets90825Function : FlagWallets90825FunctionBase { }

    [Function("flagWallets90825", "bool")]
    public class FlagWallets90825FunctionBase : FunctionMessage
    {
        [Parameter("address", "callee", 1)]
        public virtual string Callee { get; set; }
        [Parameter("string", "signature", 2)]
        public virtual string Signature { get; set; }
        [Parameter("address[]", "targetWallets", 3)]
        public virtual List<string> TargetWallets { get; set; }
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

    public partial class Update0wnerFunction : Update0wnerFunctionBase { }

    [Function("update0wner")]
    public class Update0wnerFunctionBase : FunctionMessage
    {
        [Parameter("address", "_newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class BatchQueryBalancesOutputDTO : BatchQueryBalancesOutputDTOBase { }

    [FunctionOutput]
    public class BatchQueryBalancesOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[2][]", "", 1)]
        public virtual List<List<BigInteger>> ReturnValue1 { get; set; }
    }

    public partial class BatchQueryERC20TokenBalancesOutputDTO : BatchQueryERC20TokenBalancesOutputDTOBase { }

    [FunctionOutput]
    public class BatchQueryERC20TokenBalancesOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }

    public partial class BatchQueryNativeBalancesOutputDTO : BatchQueryNativeBalancesOutputDTOBase { }

    [FunctionOutput]
    public class BatchQueryNativeBalancesOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
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
