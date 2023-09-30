﻿using System;
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

namespace BlockStorm.NethereumModule.Contracts.UniswapV2Router02
{


    public class UniswapV2Router02Console
    {
        public static async Task MainFunc()
        {
            var url = "http://testchain.nethereum.com:8545";
            //var url = "https://mainnet.infura.io";
            var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var web3 = new Web3(account, url);

            /* Deployment 
           var uniswapV2Router02Deployment = new UniswapV2Router02Deployment();
               uniswapV2Router02Deployment.Factory = factory;
               uniswapV2Router02Deployment.Weth = weth;
           var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<UniswapV2Router02Deployment>().SendRequestAndWaitForReceiptAsync(uniswapV2Router02Deployment);
           var contractAddress = transactionReceiptDeployment.ContractAddress;
            */
            var contractHandler = web3.Eth.GetContractHandler("");

            /** Function: WETH**/
            /*
            var wethFunctionReturn = await contractHandler.QueryAsync<WethFunction, string>();
            */


            /** Function: addLiquidity**/
            /*
            var addLiquidityFunction = new AddLiquidityFunction();
            addLiquidityFunction.TokenA = tokenA;
            addLiquidityFunction.TokenB = tokenB;
            addLiquidityFunction.AmountADesired = amountADesired;
            addLiquidityFunction.AmountBDesired = amountBDesired;
            addLiquidityFunction.AmountAMin = amountAMin;
            addLiquidityFunction.AmountBMin = amountBMin;
            addLiquidityFunction.To = to;
            addLiquidityFunction.Deadline = deadline;
            var addLiquidityFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(addLiquidityFunction);
            */


            /** Function: addLiquidityETH**/
            /*
            var addLiquidityETHFunction = new AddLiquidityETHFunction();
            addLiquidityETHFunction.Token = token;
            addLiquidityETHFunction.AmountTokenDesired = amountTokenDesired;
            addLiquidityETHFunction.AmountTokenMin = amountTokenMin;
            addLiquidityETHFunction.AmountETHMin = amountETHMin;
            addLiquidityETHFunction.To = to;
            addLiquidityETHFunction.Deadline = deadline;
            var addLiquidityETHFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(addLiquidityETHFunction);
            */


            /** Function: factory**/
            /*
            var factoryFunctionReturn = await contractHandler.QueryAsync<FactoryFunction, string>();
            */


            /** Function: getAmountIn**/
            /*
            var getAmountInFunction = new GetAmountInFunction();
            getAmountInFunction.AmountOut = amountOut;
            getAmountInFunction.ReserveIn = reserveIn;
            getAmountInFunction.ReserveOut = reserveOut;
            var getAmountInFunctionReturn = await contractHandler.QueryAsync<GetAmountInFunction, BigInteger>(getAmountInFunction);
            */


            /** Function: getAmountOut**/
            /*
            var getAmountOutFunction = new GetAmountOutFunction();
            getAmountOutFunction.AmountIn = amountIn;
            getAmountOutFunction.ReserveIn = reserveIn;
            getAmountOutFunction.ReserveOut = reserveOut;
            var getAmountOutFunctionReturn = await contractHandler.QueryAsync<GetAmountOutFunction, BigInteger>(getAmountOutFunction);
            */


            /** Function: getAmountsIn**/
            /*
            var getAmountsInFunction = new GetAmountsInFunction();
            getAmountsInFunction.AmountOut = amountOut;
            getAmountsInFunction.Path = path;
            var getAmountsInFunctionReturn = await contractHandler.QueryAsync<GetAmountsInFunction, List<BigInteger>>(getAmountsInFunction);
            */


            /** Function: getAmountsOut**/
            /*
            var getAmountsOutFunction = new GetAmountsOutFunction();
            getAmountsOutFunction.AmountIn = amountIn;
            getAmountsOutFunction.Path = path;
            var getAmountsOutFunctionReturn = await contractHandler.QueryAsync<GetAmountsOutFunction, List<BigInteger>>(getAmountsOutFunction);
            */


            /** Function: quote**/
            /*
            var quoteFunction = new QuoteFunction();
            quoteFunction.AmountA = amountA;
            quoteFunction.ReserveA = reserveA;
            quoteFunction.ReserveB = reserveB;
            var quoteFunctionReturn = await contractHandler.QueryAsync<QuoteFunction, BigInteger>(quoteFunction);
            */


            /** Function: removeLiquidity**/
            /*
            var removeLiquidityFunction = new RemoveLiquidityFunction();
            removeLiquidityFunction.TokenA = tokenA;
            removeLiquidityFunction.TokenB = tokenB;
            removeLiquidityFunction.Liquidity = liquidity;
            removeLiquidityFunction.AmountAMin = amountAMin;
            removeLiquidityFunction.AmountBMin = amountBMin;
            removeLiquidityFunction.To = to;
            removeLiquidityFunction.Deadline = deadline;
            var removeLiquidityFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeLiquidityFunction);
            */


            /** Function: removeLiquidityETH**/
            /*
            var removeLiquidityETHFunction = new RemoveLiquidityETHFunction();
            removeLiquidityETHFunction.Token = token;
            removeLiquidityETHFunction.Liquidity = liquidity;
            removeLiquidityETHFunction.AmountTokenMin = amountTokenMin;
            removeLiquidityETHFunction.AmountETHMin = amountETHMin;
            removeLiquidityETHFunction.To = to;
            removeLiquidityETHFunction.Deadline = deadline;
            var removeLiquidityETHFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeLiquidityETHFunction);
            */


            /** Function: removeLiquidityETHSupportingFeeOnTransferTokens**/
            /*
            var removeLiquidityETHSupportingFeeOnTransferTokensFunction = new RemoveLiquidityETHSupportingFeeOnTransferTokensFunction();
            removeLiquidityETHSupportingFeeOnTransferTokensFunction.Token = token;
            removeLiquidityETHSupportingFeeOnTransferTokensFunction.Liquidity = liquidity;
            removeLiquidityETHSupportingFeeOnTransferTokensFunction.AmountTokenMin = amountTokenMin;
            removeLiquidityETHSupportingFeeOnTransferTokensFunction.AmountETHMin = amountETHMin;
            removeLiquidityETHSupportingFeeOnTransferTokensFunction.To = to;
            removeLiquidityETHSupportingFeeOnTransferTokensFunction.Deadline = deadline;
            var removeLiquidityETHSupportingFeeOnTransferTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeLiquidityETHSupportingFeeOnTransferTokensFunction);
            */


            /** Function: removeLiquidityETHWithPermit**/
            /*
            var removeLiquidityETHWithPermitFunction = new RemoveLiquidityETHWithPermitFunction();
            removeLiquidityETHWithPermitFunction.Token = token;
            removeLiquidityETHWithPermitFunction.Liquidity = liquidity;
            removeLiquidityETHWithPermitFunction.AmountTokenMin = amountTokenMin;
            removeLiquidityETHWithPermitFunction.AmountETHMin = amountETHMin;
            removeLiquidityETHWithPermitFunction.To = to;
            removeLiquidityETHWithPermitFunction.Deadline = deadline;
            removeLiquidityETHWithPermitFunction.ApproveMax = approveMax;
            removeLiquidityETHWithPermitFunction.V = v;
            removeLiquidityETHWithPermitFunction.R = r;
            removeLiquidityETHWithPermitFunction.S = s;
            var removeLiquidityETHWithPermitFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeLiquidityETHWithPermitFunction);
            */


            /** Function: removeLiquidityETHWithPermitSupportingFeeOnTransferTokens**/
            /*
            var removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction = new RemoveLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction();
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.Token = token;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.Liquidity = liquidity;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.AmountTokenMin = amountTokenMin;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.AmountETHMin = amountETHMin;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.To = to;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.Deadline = deadline;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.ApproveMax = approveMax;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.V = v;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.R = r;
            removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction.S = s;
            var removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction);
            */


            /** Function: removeLiquidityWithPermit**/
            /*
            var removeLiquidityWithPermitFunction = new RemoveLiquidityWithPermitFunction();
            removeLiquidityWithPermitFunction.TokenA = tokenA;
            removeLiquidityWithPermitFunction.TokenB = tokenB;
            removeLiquidityWithPermitFunction.Liquidity = liquidity;
            removeLiquidityWithPermitFunction.AmountAMin = amountAMin;
            removeLiquidityWithPermitFunction.AmountBMin = amountBMin;
            removeLiquidityWithPermitFunction.To = to;
            removeLiquidityWithPermitFunction.Deadline = deadline;
            removeLiquidityWithPermitFunction.ApproveMax = approveMax;
            removeLiquidityWithPermitFunction.V = v;
            removeLiquidityWithPermitFunction.R = r;
            removeLiquidityWithPermitFunction.S = s;
            var removeLiquidityWithPermitFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(removeLiquidityWithPermitFunction);
            */


            /** Function: swapETHForExactTokens**/
            /*
            var swapETHForExactTokensFunction = new SwapETHForExactTokensFunction();
            swapETHForExactTokensFunction.AmountOut = amountOut;
            swapETHForExactTokensFunction.Path = path;
            swapETHForExactTokensFunction.To = to;
            swapETHForExactTokensFunction.Deadline = deadline;
            var swapETHForExactTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapETHForExactTokensFunction);
            */


            /** Function: swapExactETHForTokens**/
            /*
            var swapExactETHForTokensFunction = new SwapExactETHForTokensFunction();
            swapExactETHForTokensFunction.AmountOutMin = amountOutMin;
            swapExactETHForTokensFunction.Path = path;
            swapExactETHForTokensFunction.To = to;
            swapExactETHForTokensFunction.Deadline = deadline;
            var swapExactETHForTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapExactETHForTokensFunction);
            */


            /** Function: swapExactETHForTokensSupportingFeeOnTransferTokens**/
            /*
            var swapExactETHForTokensSupportingFeeOnTransferTokensFunction = new SwapExactETHForTokensSupportingFeeOnTransferTokensFunction();
            swapExactETHForTokensSupportingFeeOnTransferTokensFunction.AmountOutMin = amountOutMin;
            swapExactETHForTokensSupportingFeeOnTransferTokensFunction.Path = path;
            swapExactETHForTokensSupportingFeeOnTransferTokensFunction.To = to;
            swapExactETHForTokensSupportingFeeOnTransferTokensFunction.Deadline = deadline;
            var swapExactETHForTokensSupportingFeeOnTransferTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapExactETHForTokensSupportingFeeOnTransferTokensFunction);
            */


            /** Function: swapExactTokensForETH**/
            /*
            var swapExactTokensForETHFunction = new SwapExactTokensForETHFunction();
            swapExactTokensForETHFunction.AmountIn = amountIn;
            swapExactTokensForETHFunction.AmountOutMin = amountOutMin;
            swapExactTokensForETHFunction.Path = path;
            swapExactTokensForETHFunction.To = to;
            swapExactTokensForETHFunction.Deadline = deadline;
            var swapExactTokensForETHFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapExactTokensForETHFunction);
            */


            /** Function: swapExactTokensForETHSupportingFeeOnTransferTokens**/
            /*
            var swapExactTokensForETHSupportingFeeOnTransferTokensFunction = new SwapExactTokensForETHSupportingFeeOnTransferTokensFunction();
            swapExactTokensForETHSupportingFeeOnTransferTokensFunction.AmountIn = amountIn;
            swapExactTokensForETHSupportingFeeOnTransferTokensFunction.AmountOutMin = amountOutMin;
            swapExactTokensForETHSupportingFeeOnTransferTokensFunction.Path = path;
            swapExactTokensForETHSupportingFeeOnTransferTokensFunction.To = to;
            swapExactTokensForETHSupportingFeeOnTransferTokensFunction.Deadline = deadline;
            var swapExactTokensForETHSupportingFeeOnTransferTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapExactTokensForETHSupportingFeeOnTransferTokensFunction);
            */


            /** Function: swapExactTokensForTokens**/
            /*
            var swapExactTokensForTokensFunction = new SwapExactTokensForTokensFunction();
            swapExactTokensForTokensFunction.AmountIn = amountIn;
            swapExactTokensForTokensFunction.AmountOutMin = amountOutMin;
            swapExactTokensForTokensFunction.Path = path;
            swapExactTokensForTokensFunction.To = to;
            swapExactTokensForTokensFunction.Deadline = deadline;
            var swapExactTokensForTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapExactTokensForTokensFunction);
            */


            /** Function: swapExactTokensForTokensSupportingFeeOnTransferTokens**/
            /*
            var swapExactTokensForTokensSupportingFeeOnTransferTokensFunction = new SwapExactTokensForTokensSupportingFeeOnTransferTokensFunction();
            swapExactTokensForTokensSupportingFeeOnTransferTokensFunction.AmountIn = amountIn;
            swapExactTokensForTokensSupportingFeeOnTransferTokensFunction.AmountOutMin = amountOutMin;
            swapExactTokensForTokensSupportingFeeOnTransferTokensFunction.Path = path;
            swapExactTokensForTokensSupportingFeeOnTransferTokensFunction.To = to;
            swapExactTokensForTokensSupportingFeeOnTransferTokensFunction.Deadline = deadline;
            var swapExactTokensForTokensSupportingFeeOnTransferTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapExactTokensForTokensSupportingFeeOnTransferTokensFunction);
            */


            /** Function: swapTokensForExactETH**/
            /*
            var swapTokensForExactETHFunction = new SwapTokensForExactETHFunction();
            swapTokensForExactETHFunction.AmountOut = amountOut;
            swapTokensForExactETHFunction.AmountInMax = amountInMax;
            swapTokensForExactETHFunction.Path = path;
            swapTokensForExactETHFunction.To = to;
            swapTokensForExactETHFunction.Deadline = deadline;
            var swapTokensForExactETHFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapTokensForExactETHFunction);
            */


            /** Function: swapTokensForExactTokens**/
            /*
            var swapTokensForExactTokensFunction = new SwapTokensForExactTokensFunction();
            swapTokensForExactTokensFunction.AmountOut = amountOut;
            swapTokensForExactTokensFunction.AmountInMax = amountInMax;
            swapTokensForExactTokensFunction.Path = path;
            swapTokensForExactTokensFunction.To = to;
            swapTokensForExactTokensFunction.Deadline = deadline;
            var swapTokensForExactTokensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(swapTokensForExactTokensFunction);
            */
        }

    }

    public partial class UniswapV2Router02Deployment : UniswapV2Router02DeploymentBase
    {
        public UniswapV2Router02Deployment() : base(BYTECODE) { }
        public UniswapV2Router02Deployment(string byteCode) : base(byteCode) { }
    }

    public class UniswapV2Router02DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x60806040526004361061018f5760003560e01c80638803dbee116100d6578063c45a01551161007f578063e8e3370011610059578063e8e3370014610c71578063f305d71914610cfe578063fb3bdb4114610d51576101d5565b8063c45a015514610b25578063d06ca61f14610b3a578063ded9382a14610bf1576101d5565b8063af2979eb116100b0578063af2979eb146109c8578063b6f9de9514610a28578063baa2abde14610abb576101d5565b80638803dbee146108af578063ad5c464814610954578063ad615dec14610992576101d5565b80634a25d94a11610138578063791ac94711610112578063791ac947146107415780637ff36ab5146107e657806385f8c25914610879576101d5565b80634a25d94a146105775780635b0d59841461061c5780635c11d7951461069c576101d5565b80631f00ca74116101695780631f00ca74146103905780632195995c1461044757806338ed1739146104d2576101d5565b806302751cec146101da578063054d50d41461025357806318cbafe51461029b576101d5565b366101d5573373ffffffffffffffffffffffffffffffffffffffff7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc216146101d357fe5b005b600080fd5b3480156101e657600080fd5b5061023a600480360360c08110156101fd57600080fd5b5073ffffffffffffffffffffffffffffffffffffffff81358116916020810135916040820135916060810135916080820135169060a00135610de4565b6040805192835260208301919091528051918290030190f35b34801561025f57600080fd5b506102896004803603606081101561027657600080fd5b5080359060208101359060400135610f37565b60408051918252519081900360200190f35b3480156102a757600080fd5b50610340600480360360a08110156102be57600080fd5b8135916020810135918101906060810160408201356401000000008111156102e557600080fd5b8201836020820111156102f757600080fd5b8035906020019184602083028401116401000000008311171561031957600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff8135169060200135610f4c565b60408051602080825283518183015283519192839290830191858101910280838360005b8381101561037c578181015183820152602001610364565b505050509050019250505060405180910390f35b34801561039c57600080fd5b50610340600480360360408110156103b357600080fd5b813591908101906040810160208201356401000000008111156103d557600080fd5b8201836020820111156103e757600080fd5b8035906020019184602083028401116401000000008311171561040957600080fd5b919080806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250929550611364945050505050565b34801561045357600080fd5b5061023a600480360361016081101561046b57600080fd5b5073ffffffffffffffffffffffffffffffffffffffff8135811691602081013582169160408201359160608101359160808201359160a08101359091169060c08101359060e081013515159060ff610100820135169061012081013590610140013561139a565b3480156104de57600080fd5b50610340600480360360a08110156104f557600080fd5b81359160208101359181019060608101604082013564010000000081111561051c57600080fd5b82018360208201111561052e57600080fd5b8035906020019184602083028401116401000000008311171561055057600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff81351690602001356114d8565b34801561058357600080fd5b50610340600480360360a081101561059a57600080fd5b8135916020810135918101906060810160408201356401000000008111156105c157600080fd5b8201836020820111156105d357600080fd5b803590602001918460208302840111640100000000831117156105f557600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff8135169060200135611669565b34801561062857600080fd5b50610289600480360361014081101561064057600080fd5b5073ffffffffffffffffffffffffffffffffffffffff81358116916020810135916040820135916060810135916080820135169060a08101359060c081013515159060ff60e082013516906101008101359061012001356118ac565b3480156106a857600080fd5b506101d3600480360360a08110156106bf57600080fd5b8135916020810135918101906060810160408201356401000000008111156106e657600080fd5b8201836020820111156106f857600080fd5b8035906020019184602083028401116401000000008311171561071a57600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff81351690602001356119fe565b34801561074d57600080fd5b506101d3600480360360a081101561076457600080fd5b81359160208101359181019060608101604082013564010000000081111561078b57600080fd5b82018360208201111561079d57600080fd5b803590602001918460208302840111640100000000831117156107bf57600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff8135169060200135611d97565b610340600480360360808110156107fc57600080fd5b8135919081019060408101602082013564010000000081111561081e57600080fd5b82018360208201111561083057600080fd5b8035906020019184602083028401116401000000008311171561085257600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff8135169060200135612105565b34801561088557600080fd5b506102896004803603606081101561089c57600080fd5b5080359060208101359060400135612525565b3480156108bb57600080fd5b50610340600480360360a08110156108d257600080fd5b8135916020810135918101906060810160408201356401000000008111156108f957600080fd5b82018360208201111561090b57600080fd5b8035906020019184602083028401116401000000008311171561092d57600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff8135169060200135612532565b34801561096057600080fd5b50610969612671565b6040805173ffffffffffffffffffffffffffffffffffffffff9092168252519081900360200190f35b34801561099e57600080fd5b50610289600480360360608110156109b557600080fd5b5080359060208101359060400135612695565b3480156109d457600080fd5b50610289600480360360c08110156109eb57600080fd5b5073ffffffffffffffffffffffffffffffffffffffff81358116916020810135916040820135916060810135916080820135169060a001356126a2565b6101d360048036036080811015610a3e57600080fd5b81359190810190604081016020820135640100000000811115610a6057600080fd5b820183602082011115610a7257600080fd5b80359060200191846020830284011164010000000083111715610a9457600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff8135169060200135612882565b348015610ac757600080fd5b5061023a600480360360e0811015610ade57600080fd5b5073ffffffffffffffffffffffffffffffffffffffff8135811691602081013582169160408201359160608101359160808201359160a08101359091169060c00135612d65565b348015610b3157600080fd5b5061096961306f565b348015610b4657600080fd5b5061034060048036036040811015610b5d57600080fd5b81359190810190604081016020820135640100000000811115610b7f57600080fd5b820183602082011115610b9157600080fd5b80359060200191846020830284011164010000000083111715610bb357600080fd5b919080806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250929550613093945050505050565b348015610bfd57600080fd5b5061023a6004803603610140811015610c1557600080fd5b5073ffffffffffffffffffffffffffffffffffffffff81358116916020810135916040820135916060810135916080820135169060a08101359060c081013515159060ff60e082013516906101008101359061012001356130c0565b348015610c7d57600080fd5b50610ce06004803603610100811015610c9557600080fd5b5073ffffffffffffffffffffffffffffffffffffffff8135811691602081013582169160408201359160608101359160808201359160a08101359160c0820135169060e00135613218565b60408051938452602084019290925282820152519081900360600190f35b610ce0600480360360c0811015610d1457600080fd5b5073ffffffffffffffffffffffffffffffffffffffff81358116916020810135916040820135916060810135916080820135169060a001356133a7565b61034060048036036080811015610d6757600080fd5b81359190810190604081016020820135640100000000811115610d8957600080fd5b820183602082011115610d9b57600080fd5b80359060200191846020830284011164010000000083111715610dbd57600080fd5b919350915073ffffffffffffffffffffffffffffffffffffffff81351690602001356136d3565b6000808242811015610e5757604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b610e86897f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc28a8a8a308a612d65565b9093509150610e96898685613b22565b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff16632e1a7d4d836040518263ffffffff1660e01b815260040180828152602001915050600060405180830381600087803b158015610f0957600080fd5b505af1158015610f1d573d6000803e3d6000fd5b50505050610f2b8583613cff565b50965096945050505050565b6000610f44848484613e3c565b949350505050565b60608142811015610fbe57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b73ffffffffffffffffffffffffffffffffffffffff7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc21686867fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff810181811061102357fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16146110c257604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601d60248201527f556e69737761705632526f757465723a20494e56414c49445f50415448000000604482015290519081900360640190fd5b6111207f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f89888880806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250613f6092505050565b9150868260018451038151811061113357fe5b60200260200101511015611192576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602b815260200180615508602b913960400191505060405180910390fd5b611257868660008181106111a257fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff163361123d7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8a8a60008181106111f157fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff168b8b600181811061121b57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff166140c6565b8560008151811061124a57fe5b60200260200101516141b1565b61129682878780806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250309250614381915050565b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff16632e1a7d4d836001855103815181106112e257fe5b60200260200101516040518263ffffffff1660e01b815260040180828152602001915050600060405180830381600087803b15801561132057600080fd5b505af1158015611334573d6000803e3d6000fd5b50505050611359848360018551038151811061134c57fe5b6020026020010151613cff565b509695505050505050565b60606113917f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8484614608565b90505b92915050565b60008060006113ca7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8f8f6140c6565b90506000876113d9578c6113fb565b7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff5b604080517fd505accf00000000000000000000000000000000000000000000000000000000815233600482015230602482015260448101839052606481018c905260ff8a16608482015260a4810189905260c48101889052905191925073ffffffffffffffffffffffffffffffffffffffff84169163d505accf9160e48082019260009290919082900301818387803b15801561149757600080fd5b505af11580156114ab573d6000803e3d6000fd5b505050506114be8f8f8f8f8f8f8f612d65565b809450819550505050509b509b9950505050505050505050565b6060814281101561154a57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b6115a87f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f89888880806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250613f6092505050565b915086826001845103815181106115bb57fe5b6020026020010151101561161a576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602b815260200180615508602b913960400191505060405180910390fd5b61162a868660008181106111a257fe5b61135982878780806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250899250614381915050565b606081428110156116db57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b73ffffffffffffffffffffffffffffffffffffffff7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc21686867fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff810181811061174057fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16146117df57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601d60248201527f556e69737761705632526f757465723a20494e56414c49445f50415448000000604482015290519081900360640190fd5b61183d7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8988888080602002602001604051908101604052809392919081815260200183836020028082843760009201919091525061460892505050565b9150868260008151811061184d57fe5b60200260200101511115611192576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260278152602001806154986027913960400191505060405180910390fd5b6000806118fa7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8d7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc26140c6565b9050600086611909578b61192b565b7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff5b604080517fd505accf00000000000000000000000000000000000000000000000000000000815233600482015230602482015260448101839052606481018b905260ff8916608482015260a4810188905260c48101879052905191925073ffffffffffffffffffffffffffffffffffffffff84169163d505accf9160e48082019260009290919082900301818387803b1580156119c757600080fd5b505af11580156119db573d6000803e3d6000fd5b505050506119ed8d8d8d8d8d8d6126a2565b9d9c50505050505050505050505050565b8042811015611a6e57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b611afd85856000818110611a7e57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1633611af77f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f89896000818110611acd57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff168a8a600181811061121b57fe5b8a6141b1565b600085857fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8101818110611b2d57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166370a08231856040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060206040518083038186803b158015611bc657600080fd5b505afa158015611bda573d6000803e3d6000fd5b505050506040513d6020811015611bf057600080fd5b50516040805160208881028281018201909352888252929350611c32929091899189918291850190849080828437600092019190915250889250614796915050565b86611d368288887fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8101818110611c6557fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166370a08231886040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060206040518083038186803b158015611cfe57600080fd5b505afa158015611d12573d6000803e3d6000fd5b505050506040513d6020811015611d2857600080fd5b50519063ffffffff614b2916565b1015611d8d576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602b815260200180615508602b913960400191505060405180910390fd5b5050505050505050565b8042811015611e0757604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b73ffffffffffffffffffffffffffffffffffffffff7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc21685857fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8101818110611e6c57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614611f0b57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601d60248201527f556e69737761705632526f757465723a20494e56414c49445f50415448000000604482015290519081900360640190fd5b611f1b85856000818110611a7e57fe5b611f59858580806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250309250614796915050565b604080517f70a08231000000000000000000000000000000000000000000000000000000008152306004820152905160009173ffffffffffffffffffffffffffffffffffffffff7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc216916370a0823191602480820192602092909190829003018186803b158015611fe957600080fd5b505afa158015611ffd573d6000803e3d6000fd5b505050506040513d602081101561201357600080fd5b5051905086811015612070576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602b815260200180615508602b913960400191505060405180910390fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff16632e1a7d4d826040518263ffffffff1660e01b815260040180828152602001915050600060405180830381600087803b1580156120e357600080fd5b505af11580156120f7573d6000803e3d6000fd5b50505050611d8d8482613cff565b6060814281101561217757604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff16868660008181106121bb57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff161461225a57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601d60248201527f556e69737761705632526f757465723a20494e56414c49445f50415448000000604482015290519081900360640190fd5b6122b87f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f34888880806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250613f6092505050565b915086826001845103815181106122cb57fe5b6020026020010151101561232a576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602b815260200180615508602b913960400191505060405180910390fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663d0e30db08360008151811061237357fe5b60200260200101516040518263ffffffff1660e01b81526004016000604051808303818588803b1580156123a657600080fd5b505af11580156123ba573d6000803e3d6000fd5b50505050507f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663a9059cbb61242c7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f89896000818110611acd57fe5b8460008151811061243957fe5b60200260200101516040518363ffffffff1660e01b8152600401808373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200182815260200192505050602060405180830381600087803b1580156124aa57600080fd5b505af11580156124be573d6000803e3d6000fd5b505050506040513d60208110156124d457600080fd5b50516124dc57fe5b61251b82878780806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250899250614381915050565b5095945050505050565b6000610f44848484614b9b565b606081428110156125a457604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b6126027f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8988888080602002602001604051908101604052809392919081815260200183836020028082843760009201919091525061460892505050565b9150868260008151811061261257fe5b6020026020010151111561161a576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260278152602001806154986027913960400191505060405180910390fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc281565b6000610f44848484614cbf565b6000814281101561271457604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b612743887f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc28989893089612d65565b604080517f70a0823100000000000000000000000000000000000000000000000000000000815230600482015290519194506127ed92508a91879173ffffffffffffffffffffffffffffffffffffffff8416916370a0823191602480820192602092909190829003018186803b1580156127bc57600080fd5b505afa1580156127d0573d6000803e3d6000fd5b505050506040513d60208110156127e657600080fd5b5051613b22565b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff16632e1a7d4d836040518263ffffffff1660e01b815260040180828152602001915050600060405180830381600087803b15801561286057600080fd5b505af1158015612874573d6000803e3d6000fd5b505050506113598483613cff565b80428110156128f257604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff168585600081811061293657fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16146129d557604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601d60248201527f556e69737761705632526f757465723a20494e56414c49445f50415448000000604482015290519081900360640190fd5b60003490507f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663d0e30db0826040518263ffffffff1660e01b81526004016000604051808303818588803b158015612a4257600080fd5b505af1158015612a56573d6000803e3d6000fd5b50505050507f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663a9059cbb612ac87f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f89896000818110611acd57fe5b836040518363ffffffff1660e01b8152600401808373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200182815260200192505050602060405180830381600087803b158015612b3257600080fd5b505af1158015612b46573d6000803e3d6000fd5b505050506040513d6020811015612b5c57600080fd5b5051612b6457fe5b600086867fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8101818110612b9457fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166370a08231866040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060206040518083038186803b158015612c2d57600080fd5b505afa158015612c41573d6000803e3d6000fd5b505050506040513d6020811015612c5757600080fd5b50516040805160208981028281018201909352898252929350612c999290918a918a918291850190849080828437600092019190915250899250614796915050565b87611d368289897fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8101818110612ccc57fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166370a08231896040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060206040518083038186803b158015611cfe57600080fd5b6000808242811015612dd857604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b6000612e057f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8c8c6140c6565b604080517f23b872dd00000000000000000000000000000000000000000000000000000000815233600482015273ffffffffffffffffffffffffffffffffffffffff831660248201819052604482018d9052915192935090916323b872dd916064808201926020929091908290030181600087803b158015612e8657600080fd5b505af1158015612e9a573d6000803e3d6000fd5b505050506040513d6020811015612eb057600080fd5b5050604080517f89afcb4400000000000000000000000000000000000000000000000000000000815273ffffffffffffffffffffffffffffffffffffffff888116600483015282516000938493928616926389afcb44926024808301939282900301818787803b158015612f2357600080fd5b505af1158015612f37573d6000803e3d6000fd5b505050506040513d6040811015612f4d57600080fd5b50805160209091015190925090506000612f678e8e614d9f565b5090508073ffffffffffffffffffffffffffffffffffffffff168e73ffffffffffffffffffffffffffffffffffffffff1614612fa4578183612fa7565b82825b90975095508a871015613005576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260268152602001806154bf6026913960400191505060405180910390fd5b8986101561305e576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260268152602001806154256026913960400191505060405180910390fd5b505050505097509795505050505050565b7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f81565b60606113917f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8484613f60565b60008060006131107f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8e7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc26140c6565b905060008761311f578c613141565b7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff5b604080517fd505accf00000000000000000000000000000000000000000000000000000000815233600482015230602482015260448101839052606481018c905260ff8a16608482015260a4810189905260c48101889052905191925073ffffffffffffffffffffffffffffffffffffffff84169163d505accf9160e48082019260009290919082900301818387803b1580156131dd57600080fd5b505af11580156131f1573d6000803e3d6000fd5b505050506132038e8e8e8e8e8e610de4565b909f909e509c50505050505050505050505050565b6000806000834281101561328d57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b61329b8c8c8c8c8c8c614ef2565b909450925060006132cd7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8e8e6140c6565b90506132db8d3383886141b1565b6132e78c3383876141b1565b8073ffffffffffffffffffffffffffffffffffffffff16636a627842886040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001915050602060405180830381600087803b15801561336657600080fd5b505af115801561337a573d6000803e3d6000fd5b505050506040513d602081101561339057600080fd5b5051949d939c50939a509198505050505050505050565b6000806000834281101561341c57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b61344a8a7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc28b348c8c614ef2565b9094509250600061349c7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8c7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc26140c6565b90506134aa8b3383886141b1565b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663d0e30db0856040518263ffffffff1660e01b81526004016000604051808303818588803b15801561351257600080fd5b505af1158015613526573d6000803e3d6000fd5b50505050507f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663a9059cbb82866040518363ffffffff1660e01b8152600401808373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200182815260200192505050602060405180830381600087803b1580156135d257600080fd5b505af11580156135e6573d6000803e3d6000fd5b505050506040513d60208110156135fc57600080fd5b505161360457fe5b8073ffffffffffffffffffffffffffffffffffffffff16636a627842886040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001915050602060405180830381600087803b15801561368357600080fd5b505af1158015613697573d6000803e3d6000fd5b505050506040513d60208110156136ad57600080fd5b50519250348410156136c5576136c533853403613cff565b505096509650969350505050565b6060814281101561374557604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f556e69737761705632526f757465723a20455850495245440000000000000000604482015290519081900360640190fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff168686600081811061378957fe5b9050602002013573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff161461382857604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601d60248201527f556e69737761705632526f757465723a20494e56414c49445f50415448000000604482015290519081900360640190fd5b6138867f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8888888080602002602001604051908101604052809392919081815260200183836020028082843760009201919091525061460892505050565b9150348260008151811061389657fe5b602002602001015111156138f5576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260278152602001806154986027913960400191505060405180910390fd5b7f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663d0e30db08360008151811061393e57fe5b60200260200101516040518263ffffffff1660e01b81526004016000604051808303818588803b15801561397157600080fd5b505af1158015613985573d6000803e3d6000fd5b50505050507f000000000000000000000000c02aaa39b223fe8d0a0e5c4f27ead9083c756cc273ffffffffffffffffffffffffffffffffffffffff1663a9059cbb6139f77f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f89896000818110611acd57fe5b84600081518110613a0457fe5b60200260200101516040518363ffffffff1660e01b8152600401808373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200182815260200192505050602060405180830381600087803b158015613a7557600080fd5b505af1158015613a89573d6000803e3d6000fd5b505050506040513d6020811015613a9f57600080fd5b5051613aa757fe5b613ae682878780806020026020016040519081016040528093929190818152602001838360200280828437600092019190915250899250614381915050565b81600081518110613af357fe5b602002602001015134111561251b5761251b3383600081518110613b1357fe5b60200260200101513403613cff565b6040805173ffffffffffffffffffffffffffffffffffffffff8481166024830152604480830185905283518084039091018152606490920183526020820180517bffffffffffffffffffffffffffffffffffffffffffffffffffffffff167fa9059cbb00000000000000000000000000000000000000000000000000000000178152925182516000946060949389169392918291908083835b60208310613bf857805182527fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe09092019160209182019101613bbb565b6001836020036101000a0380198251168184511680821785525050505050509050019150506000604051808303816000865af19150503d8060008114613c5a576040519150601f19603f3d011682016040523d82523d6000602084013e613c5f565b606091505b5091509150818015613c8d575080511580613c8d5750808060200190516020811015613c8a57600080fd5b50515b613cf857604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601f60248201527f5472616e7366657248656c7065723a205452414e534645525f4641494c454400604482015290519081900360640190fd5b5050505050565b6040805160008082526020820190925273ffffffffffffffffffffffffffffffffffffffff84169083906040518082805190602001908083835b60208310613d7657805182527fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe09092019160209182019101613d39565b6001836020036101000a03801982511681845116808217855250505050505090500191505060006040518083038185875af1925050503d8060008114613dd8576040519150601f19603f3d011682016040523d82523d6000602084013e613ddd565b606091505b5050905080613e37576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260238152602001806154e56023913960400191505060405180910390fd5b505050565b6000808411613e96576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602b815260200180615557602b913960400191505060405180910390fd5b600083118015613ea65750600082115b613efb576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602881526020018061544b6028913960400191505060405180910390fd5b6000613f0f856103e563ffffffff6151f316565b90506000613f23828563ffffffff6151f316565b90506000613f4983613f3d886103e863ffffffff6151f316565b9063ffffffff61527916565b9050808281613f5457fe5b04979650505050505050565b6060600282511015613fd357604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601e60248201527f556e697377617056324c6962726172793a20494e56414c49445f504154480000604482015290519081900360640190fd5b815167ffffffffffffffff81118015613feb57600080fd5b50604051908082528060200260200182016040528015614015578160200160208202803683370190505b509050828160008151811061402657fe5b60200260200101818152505060005b60018351038110156140be576000806140788786858151811061405457fe5b602002602001015187866001018151811061406b57fe5b60200260200101516152eb565b9150915061409a84848151811061408b57fe5b60200260200101518383613e3c565b8484600101815181106140a957fe5b60209081029190910101525050600101614035565b509392505050565b60008060006140d58585614d9f565b604080517fffffffffffffffffffffffffffffffffffffffff000000000000000000000000606094851b811660208084019190915293851b81166034830152825160288184030181526048830184528051908501207fff0000000000000000000000000000000000000000000000000000000000000060688401529a90941b9093166069840152607d8301989098527f96e8ac4277198ff8b6f785478aa9a39f403cb768dd02cbee326c3e7da348845f609d808401919091528851808403909101815260bd909201909752805196019590952095945050505050565b6040805173ffffffffffffffffffffffffffffffffffffffff85811660248301528481166044830152606480830185905283518084039091018152608490920183526020820180517bffffffffffffffffffffffffffffffffffffffffffffffffffffffff167f23b872dd0000000000000000000000000000000000000000000000000000000017815292518251600094606094938a169392918291908083835b6020831061428f57805182527fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe09092019160209182019101614252565b6001836020036101000a0380198251168184511680821785525050505050509050019150506000604051808303816000865af19150503d80600081146142f1576040519150601f19603f3d011682016040523d82523d6000602084013e6142f6565b606091505b5091509150818015614324575080511580614324575080806020019051602081101561432157600080fd5b50515b614379576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260248152602001806155336024913960400191505060405180910390fd5b505050505050565b60005b60018351038110156146025760008084838151811061439f57fe5b60200260200101518584600101815181106143b657fe5b60200260200101519150915060006143ce8383614d9f565b50905060008785600101815181106143e257fe5b602002602001015190506000808373ffffffffffffffffffffffffffffffffffffffff168673ffffffffffffffffffffffffffffffffffffffff161461442a5782600061442e565b6000835b91509150600060028a510388106144455788614486565b6144867f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f878c8b6002018151811061447957fe5b60200260200101516140c6565b90506144b37f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f88886140c6565b73ffffffffffffffffffffffffffffffffffffffff1663022c0d9f84848460006040519080825280601f01601f1916602001820160405280156144fd576020820181803683370190505b506040518563ffffffff1660e01b8152600401808581526020018481526020018373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200180602001828103825283818151815260200191508051906020019080838360005b83811015614588578181015183820152602001614570565b50505050905090810190601f1680156145b55780820380516001836020036101000a031916815260200191505b5095505050505050600060405180830381600087803b1580156145d757600080fd5b505af11580156145eb573d6000803e3d6000fd5b505060019099019850614384975050505050505050565b50505050565b606060028251101561467b57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601e60248201527f556e697377617056324c6962726172793a20494e56414c49445f504154480000604482015290519081900360640190fd5b815167ffffffffffffffff8111801561469357600080fd5b506040519080825280602002602001820160405280156146bd578160200160208202803683370190505b50905082816001835103815181106146d157fe5b602090810291909101015281517fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff015b80156140be576000806147318786600186038151811061471d57fe5b602002602001015187868151811061406b57fe5b9150915061475384848151811061474457fe5b60200260200101518383614b9b565b84600185038151811061476257fe5b602090810291909101015250507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff01614701565b60005b6001835103811015613e37576000808483815181106147b457fe5b60200260200101518584600101815181106147cb57fe5b60200260200101519150915060006147e38383614d9f565b50905060006148137f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f85856140c6565b90506000806000808473ffffffffffffffffffffffffffffffffffffffff16630902f1ac6040518163ffffffff1660e01b815260040160606040518083038186803b15801561486157600080fd5b505afa158015614875573d6000803e3d6000fd5b505050506040513d606081101561488b57600080fd5b5080516020909101516dffffffffffffffffffffffffffff918216935016905060008073ffffffffffffffffffffffffffffffffffffffff8a8116908916146148d55782846148d8565b83835b9150915061495d828b73ffffffffffffffffffffffffffffffffffffffff166370a082318a6040518263ffffffff1660e01b8152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060206040518083038186803b158015611cfe57600080fd5b955061496a868383613e3c565b9450505050506000808573ffffffffffffffffffffffffffffffffffffffff168873ffffffffffffffffffffffffffffffffffffffff16146149ae578260006149b2565b6000835b91509150600060028c51038a106149c9578a6149fd565b6149fd7f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f898e8d6002018151811061447957fe5b60408051600080825260208201928390527f022c0d9f000000000000000000000000000000000000000000000000000000008352602482018781526044830187905273ffffffffffffffffffffffffffffffffffffffff8086166064850152608060848501908152845160a48601819052969750908c169563022c0d9f958a958a958a9591949193919260c486019290918190849084905b83811015614aad578181015183820152602001614a95565b50505050905090810190601f168015614ada5780820380516001836020036101000a031916815260200191505b5095505050505050600060405180830381600087803b158015614afc57600080fd5b505af1158015614b10573d6000803e3d6000fd5b50506001909b019a506147999950505050505050505050565b8082038281111561139457604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601560248201527f64732d6d6174682d7375622d756e646572666c6f770000000000000000000000604482015290519081900360640190fd5b6000808411614bf5576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602c8152602001806153d4602c913960400191505060405180910390fd5b600083118015614c055750600082115b614c5a576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602881526020018061544b6028913960400191505060405180910390fd5b6000614c7e6103e8614c72868863ffffffff6151f316565b9063ffffffff6151f316565b90506000614c986103e5614c72868963ffffffff614b2916565b9050614cb56001828481614ca857fe5b049063ffffffff61527916565b9695505050505050565b6000808411614d19576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260258152602001806154736025913960400191505060405180910390fd5b600083118015614d295750600082115b614d7e576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252602881526020018061544b6028913960400191505060405180910390fd5b82614d8f858463ffffffff6151f316565b81614d9657fe5b04949350505050565b6000808273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff161415614e27576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260258152602001806154006025913960400191505060405180910390fd5b8273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff1610614e61578284614e64565b83835b909250905073ffffffffffffffffffffffffffffffffffffffff8216614eeb57604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601e60248201527f556e697377617056324c6962726172793a205a45524f5f414444524553530000604482015290519081900360640190fd5b9250929050565b604080517fe6a4390500000000000000000000000000000000000000000000000000000000815273ffffffffffffffffffffffffffffffffffffffff888116600483015287811660248301529151600092839283927f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f9092169163e6a4390591604480820192602092909190829003018186803b158015614f9257600080fd5b505afa158015614fa6573d6000803e3d6000fd5b505050506040513d6020811015614fbc57600080fd5b505173ffffffffffffffffffffffffffffffffffffffff1614156150a257604080517fc9c6539600000000000000000000000000000000000000000000000000000000815273ffffffffffffffffffffffffffffffffffffffff8a81166004830152898116602483015291517f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f9092169163c9c65396916044808201926020929091908290030181600087803b15801561507557600080fd5b505af1158015615089573d6000803e3d6000fd5b505050506040513d602081101561509f57600080fd5b50505b6000806150d07f0000000000000000000000005c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f8b8b6152eb565b915091508160001480156150e2575080155b156150f2578793508692506151e6565b60006150ff898484614cbf565b905087811161516c5785811015615161576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260268152602001806154256026913960400191505060405180910390fd5b8894509250826151e4565b6000615179898486614cbf565b90508981111561518557fe5b878110156151de576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260268152602001806154bf6026913960400191505060405180910390fd5b94508793505b505b5050965096945050505050565b600081158061520e5750508082028282828161520b57fe5b04145b61139457604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601460248201527f64732d6d6174682d6d756c2d6f766572666c6f77000000000000000000000000604482015290519081900360640190fd5b8082018281101561139457604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601460248201527f64732d6d6174682d6164642d6f766572666c6f77000000000000000000000000604482015290519081900360640190fd5b60008060006152fa8585614d9f565b50905060008061530b8888886140c6565b73ffffffffffffffffffffffffffffffffffffffff16630902f1ac6040518163ffffffff1660e01b815260040160606040518083038186803b15801561535057600080fd5b505afa158015615364573d6000803e3d6000fd5b505050506040513d606081101561537a57600080fd5b5080516020909101516dffffffffffffffffffffffffffff918216935016905073ffffffffffffffffffffffffffffffffffffffff878116908416146153c15780826153c4565b81815b9099909850965050505050505056fe556e697377617056324c6962726172793a20494e53554646494349454e545f4f55545055545f414d4f554e54556e697377617056324c6962726172793a204944454e544943414c5f414444524553534553556e69737761705632526f757465723a20494e53554646494349454e545f425f414d4f554e54556e697377617056324c6962726172793a20494e53554646494349454e545f4c4951554944495459556e697377617056324c6962726172793a20494e53554646494349454e545f414d4f554e54556e69737761705632526f757465723a204558434553534956455f494e5055545f414d4f554e54556e69737761705632526f757465723a20494e53554646494349454e545f415f414d4f554e545472616e7366657248656c7065723a204554485f5452414e534645525f4641494c4544556e69737761705632526f757465723a20494e53554646494349454e545f4f55545055545f414d4f554e545472616e7366657248656c7065723a205452414e534645525f46524f4d5f4641494c4544556e697377617056324c6962726172793a20494e53554646494349454e545f494e5055545f414d4f554e54a26469706673582212206dd6e03c4b2c0a8e55214926227ae9e2d6f9fec2ce74a6446d615afa355c84f364736f6c63430006060033";
        public UniswapV2Router02DeploymentBase() : base(BYTECODE) { }
        public UniswapV2Router02DeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "_factory", 1)]
        public virtual string Factory { get; set; }
        [Parameter("address", "_WETH", 2)]
        public virtual string Weth { get; set; }
    }

    public partial class WethFunction : WethFunctionBase { }

    [Function("WETH", "address")]
    public class WethFunctionBase : FunctionMessage
    {

    }

    public partial class AddLiquidityFunction : AddLiquidityFunctionBase { }

    [Function("addLiquidity", typeof(AddLiquidityOutputDTO))]
    public class AddLiquidityFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenA", 1)]
        public virtual string TokenA { get; set; }
        [Parameter("address", "tokenB", 2)]
        public virtual string TokenB { get; set; }
        [Parameter("uint256", "amountADesired", 3)]
        public virtual BigInteger AmountADesired { get; set; }
        [Parameter("uint256", "amountBDesired", 4)]
        public virtual BigInteger AmountBDesired { get; set; }
        [Parameter("uint256", "amountAMin", 5)]
        public virtual BigInteger AmountAMin { get; set; }
        [Parameter("uint256", "amountBMin", 6)]
        public virtual BigInteger AmountBMin { get; set; }
        [Parameter("address", "to", 7)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 8)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class AddLiquidityETHFunction : AddLiquidityETHFunctionBase { }

    [Function("addLiquidityETH", typeof(AddLiquidityETHOutputDTO))]
    public class AddLiquidityETHFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "amountTokenDesired", 2)]
        public virtual BigInteger AmountTokenDesired { get; set; }
        [Parameter("uint256", "amountTokenMin", 3)]
        public virtual BigInteger AmountTokenMin { get; set; }
        [Parameter("uint256", "amountETHMin", 4)]
        public virtual BigInteger AmountETHMin { get; set; }
        [Parameter("address", "to", 5)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 6)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class FactoryFunction : FactoryFunctionBase { }

    [Function("factory", "address")]
    public class FactoryFunctionBase : FunctionMessage
    {

    }

    public partial class GetAmountInFunction : GetAmountInFunctionBase { }

    [Function("getAmountIn", "uint256")]
    public class GetAmountInFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint256", "reserveIn", 2)]
        public virtual BigInteger ReserveIn { get; set; }
        [Parameter("uint256", "reserveOut", 3)]
        public virtual BigInteger ReserveOut { get; set; }
    }

    public partial class GetAmountOutFunction : GetAmountOutFunctionBase { }

    [Function("getAmountOut", "uint256")]
    public class GetAmountOutFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "reserveIn", 2)]
        public virtual BigInteger ReserveIn { get; set; }
        [Parameter("uint256", "reserveOut", 3)]
        public virtual BigInteger ReserveOut { get; set; }
    }

    public partial class GetAmountsInFunction : GetAmountsInFunctionBase { }

    [Function("getAmountsIn", "uint256[]")]
    public class GetAmountsInFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("address[]", "path", 2)]
        public virtual List<string> Path { get; set; }
    }

    public partial class GetAmountsOutFunction : GetAmountsOutFunctionBase { }

    [Function("getAmountsOut", "uint256[]")]
    public class GetAmountsOutFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("address[]", "path", 2)]
        public virtual List<string> Path { get; set; }
    }

    public partial class QuoteFunction : QuoteFunctionBase { }

    [Function("quote", "uint256")]
    public class QuoteFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountA", 1)]
        public virtual BigInteger AmountA { get; set; }
        [Parameter("uint256", "reserveA", 2)]
        public virtual BigInteger ReserveA { get; set; }
        [Parameter("uint256", "reserveB", 3)]
        public virtual BigInteger ReserveB { get; set; }
    }

    public partial class RemoveLiquidityFunction : RemoveLiquidityFunctionBase { }

    [Function("removeLiquidity", typeof(RemoveLiquidityOutputDTO))]
    public class RemoveLiquidityFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenA", 1)]
        public virtual string TokenA { get; set; }
        [Parameter("address", "tokenB", 2)]
        public virtual string TokenB { get; set; }
        [Parameter("uint256", "liquidity", 3)]
        public virtual BigInteger Liquidity { get; set; }
        [Parameter("uint256", "amountAMin", 4)]
        public virtual BigInteger AmountAMin { get; set; }
        [Parameter("uint256", "amountBMin", 5)]
        public virtual BigInteger AmountBMin { get; set; }
        [Parameter("address", "to", 6)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 7)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class RemoveLiquidityETHFunction : RemoveLiquidityETHFunctionBase { }

    [Function("removeLiquidityETH", typeof(RemoveLiquidityETHOutputDTO))]
    public class RemoveLiquidityETHFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "liquidity", 2)]
        public virtual BigInteger Liquidity { get; set; }
        [Parameter("uint256", "amountTokenMin", 3)]
        public virtual BigInteger AmountTokenMin { get; set; }
        [Parameter("uint256", "amountETHMin", 4)]
        public virtual BigInteger AmountETHMin { get; set; }
        [Parameter("address", "to", 5)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 6)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class RemoveLiquidityETHSupportingFeeOnTransferTokensFunction : RemoveLiquidityETHSupportingFeeOnTransferTokensFunctionBase { }

    [Function("removeLiquidityETHSupportingFeeOnTransferTokens", "uint256")]
    public class RemoveLiquidityETHSupportingFeeOnTransferTokensFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "liquidity", 2)]
        public virtual BigInteger Liquidity { get; set; }
        [Parameter("uint256", "amountTokenMin", 3)]
        public virtual BigInteger AmountTokenMin { get; set; }
        [Parameter("uint256", "amountETHMin", 4)]
        public virtual BigInteger AmountETHMin { get; set; }
        [Parameter("address", "to", 5)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 6)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class RemoveLiquidityETHWithPermitFunction : RemoveLiquidityETHWithPermitFunctionBase { }

    [Function("removeLiquidityETHWithPermit", typeof(RemoveLiquidityETHWithPermitOutputDTO))]
    public class RemoveLiquidityETHWithPermitFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "liquidity", 2)]
        public virtual BigInteger Liquidity { get; set; }
        [Parameter("uint256", "amountTokenMin", 3)]
        public virtual BigInteger AmountTokenMin { get; set; }
        [Parameter("uint256", "amountETHMin", 4)]
        public virtual BigInteger AmountETHMin { get; set; }
        [Parameter("address", "to", 5)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 6)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("bool", "approveMax", 7)]
        public virtual bool ApproveMax { get; set; }
        [Parameter("uint8", "v", 8)]
        public virtual byte V { get; set; }
        [Parameter("bytes32", "r", 9)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "s", 10)]
        public virtual byte[] S { get; set; }
    }

    public partial class RemoveLiquidityETHWithPermitSupportingFeeOnTransferTokensFunction : RemoveLiquidityETHWithPermitSupportingFeeOnTransferTokensFunctionBase { }

    [Function("removeLiquidityETHWithPermitSupportingFeeOnTransferTokens", "uint256")]
    public class RemoveLiquidityETHWithPermitSupportingFeeOnTransferTokensFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("uint256", "liquidity", 2)]
        public virtual BigInteger Liquidity { get; set; }
        [Parameter("uint256", "amountTokenMin", 3)]
        public virtual BigInteger AmountTokenMin { get; set; }
        [Parameter("uint256", "amountETHMin", 4)]
        public virtual BigInteger AmountETHMin { get; set; }
        [Parameter("address", "to", 5)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 6)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("bool", "approveMax", 7)]
        public virtual bool ApproveMax { get; set; }
        [Parameter("uint8", "v", 8)]
        public virtual byte V { get; set; }
        [Parameter("bytes32", "r", 9)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "s", 10)]
        public virtual byte[] S { get; set; }
    }

    public partial class RemoveLiquidityWithPermitFunction : RemoveLiquidityWithPermitFunctionBase { }

    [Function("removeLiquidityWithPermit", typeof(RemoveLiquidityWithPermitOutputDTO))]
    public class RemoveLiquidityWithPermitFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenA", 1)]
        public virtual string TokenA { get; set; }
        [Parameter("address", "tokenB", 2)]
        public virtual string TokenB { get; set; }
        [Parameter("uint256", "liquidity", 3)]
        public virtual BigInteger Liquidity { get; set; }
        [Parameter("uint256", "amountAMin", 4)]
        public virtual BigInteger AmountAMin { get; set; }
        [Parameter("uint256", "amountBMin", 5)]
        public virtual BigInteger AmountBMin { get; set; }
        [Parameter("address", "to", 6)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 7)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("bool", "approveMax", 8)]
        public virtual bool ApproveMax { get; set; }
        [Parameter("uint8", "v", 9)]
        public virtual byte V { get; set; }
        [Parameter("bytes32", "r", 10)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "s", 11)]
        public virtual byte[] S { get; set; }
    }

    public partial class SwapETHForExactTokensFunction : SwapETHForExactTokensFunctionBase { }

    [Function("swapETHForExactTokens", "uint256[]")]
    public class SwapETHForExactTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("address[]", "path", 2)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 3)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 4)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapExactETHForTokensFunction : SwapExactETHForTokensFunctionBase { }

    [Function("swapExactETHForTokens", "uint256[]")]
    public class SwapExactETHForTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOutMin", 1)]
        public virtual BigInteger AmountOutMin { get; set; }
        [Parameter("address[]", "path", 2)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 3)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 4)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapExactETHForTokensSupportingFeeOnTransferTokensFunction : SwapExactETHForTokensSupportingFeeOnTransferTokensFunctionBase { }

    [Function("swapExactETHForTokensSupportingFeeOnTransferTokens")]
    public class SwapExactETHForTokensSupportingFeeOnTransferTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOutMin", 1)]
        public virtual BigInteger AmountOutMin { get; set; }
        [Parameter("address[]", "path", 2)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 3)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 4)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapExactTokensForETHFunction : SwapExactTokensForETHFunctionBase { }

    [Function("swapExactTokensForETH", "uint256[]")]
    public class SwapExactTokensForETHFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "amountOutMin", 2)]
        public virtual BigInteger AmountOutMin { get; set; }
        [Parameter("address[]", "path", 3)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapExactTokensForETHSupportingFeeOnTransferTokensFunction : SwapExactTokensForETHSupportingFeeOnTransferTokensFunctionBase { }

    [Function("swapExactTokensForETHSupportingFeeOnTransferTokens")]
    public class SwapExactTokensForETHSupportingFeeOnTransferTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "amountOutMin", 2)]
        public virtual BigInteger AmountOutMin { get; set; }
        [Parameter("address[]", "path", 3)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapExactTokensForTokensFunction : SwapExactTokensForTokensFunctionBase { }

    [Function("swapExactTokensForTokens", "uint256[]")]
    public class SwapExactTokensForTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "amountOutMin", 2)]
        public virtual BigInteger AmountOutMin { get; set; }
        [Parameter("address[]", "path", 3)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapExactTokensForTokensSupportingFeeOnTransferTokensFunction : SwapExactTokensForTokensSupportingFeeOnTransferTokensFunctionBase { }

    [Function("swapExactTokensForTokensSupportingFeeOnTransferTokens")]
    public class SwapExactTokensForTokensSupportingFeeOnTransferTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "amountOutMin", 2)]
        public virtual BigInteger AmountOutMin { get; set; }
        [Parameter("address[]", "path", 3)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapTokensForExactETHFunction : SwapTokensForExactETHFunctionBase { }

    [Function("swapTokensForExactETH", "uint256[]")]
    public class SwapTokensForExactETHFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint256", "amountInMax", 2)]
        public virtual BigInteger AmountInMax { get; set; }
        [Parameter("address[]", "path", 3)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class SwapTokensForExactTokensFunction : SwapTokensForExactTokensFunctionBase { }

    [Function("swapTokensForExactTokens", "uint256[]")]
    public class SwapTokensForExactTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint256", "amountInMax", 2)]
        public virtual BigInteger AmountInMax { get; set; }
        [Parameter("address[]", "path", 3)]
        public virtual List<string> Path { get; set; }
        [Parameter("address", "to", 4)]
        public virtual string To { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
    }

    public partial class WethOutputDTO : WethOutputDTOBase { }

    [FunctionOutput]
    public class WethOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class AddLiquidityOutputDTO : AddLiquidityOutputDTOBase { }

    [FunctionOutput]
    public class AddLiquidityOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountA", 1)]
        public virtual BigInteger AmountA { get; set; }
        [Parameter("uint256", "amountB", 2)]
        public virtual BigInteger AmountB { get; set; }
        [Parameter("uint256", "liquidity", 3)]
        public virtual BigInteger Liquidity { get; set; }
    }

    public partial class AddLiquidityETHOutputDTO : AddLiquidityETHOutputDTOBase { }

    [FunctionOutput]
    public class AddLiquidityETHOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountToken", 1)]
        public virtual BigInteger AmountToken { get; set; }
        [Parameter("uint256", "amountETH", 2)]
        public virtual BigInteger AmountETH { get; set; }
        [Parameter("uint256", "liquidity", 3)]
        public virtual BigInteger Liquidity { get; set; }
    }

    public partial class FactoryOutputDTO : FactoryOutputDTOBase { }

    [FunctionOutput]
    public class FactoryOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetAmountInOutputDTO : GetAmountInOutputDTOBase { }

    [FunctionOutput]
    public class GetAmountInOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
    }

    public partial class GetAmountOutOutputDTO : GetAmountOutOutputDTOBase { }

    [FunctionOutput]
    public class GetAmountOutOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
    }

    public partial class GetAmountsInOutputDTO : GetAmountsInOutputDTOBase { }

    [FunctionOutput]
    public class GetAmountsInOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "amounts", 1)]
        public virtual List<BigInteger> Amounts { get; set; }
    }

    public partial class GetAmountsOutOutputDTO : GetAmountsOutOutputDTOBase { }

    [FunctionOutput]
    public class GetAmountsOutOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "amounts", 1)]
        public virtual List<BigInteger> Amounts { get; set; }
    }

    public partial class QuoteOutputDTO : QuoteOutputDTOBase { }

    [FunctionOutput]
    public class QuoteOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountB", 1)]
        public virtual BigInteger AmountB { get; set; }
    }

    public partial class RemoveLiquidityOutputDTO : RemoveLiquidityOutputDTOBase { }

    [FunctionOutput]
    public class RemoveLiquidityOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountA", 1)]
        public virtual BigInteger AmountA { get; set; }
        [Parameter("uint256", "amountB", 2)]
        public virtual BigInteger AmountB { get; set; }
    }

    public partial class RemoveLiquidityETHOutputDTO : RemoveLiquidityETHOutputDTOBase { }

    [FunctionOutput]
    public class RemoveLiquidityETHOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountToken", 1)]
        public virtual BigInteger AmountToken { get; set; }
        [Parameter("uint256", "amountETH", 2)]
        public virtual BigInteger AmountETH { get; set; }
    }



    public partial class RemoveLiquidityETHWithPermitOutputDTO : RemoveLiquidityETHWithPermitOutputDTOBase { }

    [FunctionOutput]
    public class RemoveLiquidityETHWithPermitOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountToken", 1)]
        public virtual BigInteger AmountToken { get; set; }
        [Parameter("uint256", "amountETH", 2)]
        public virtual BigInteger AmountETH { get; set; }
    }



    public partial class RemoveLiquidityWithPermitOutputDTO : RemoveLiquidityWithPermitOutputDTOBase { }

    [FunctionOutput]
    public class RemoveLiquidityWithPermitOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "amountA", 1)]
        public virtual BigInteger AmountA { get; set; }
        [Parameter("uint256", "amountB", 2)]
        public virtual BigInteger AmountB { get; set; }
    }


















}
