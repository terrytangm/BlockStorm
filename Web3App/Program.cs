﻿// See https://aka.ms/new-console-template for more information
using BlockStorm.EFModels;
using BlockStorm.Samples.Contracts.UniswapV2Factory;
using BlockStorm.Samples.Contracts.UniswapV2Pair;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC;
using Nethereum.Quorum;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Web3Accounts=Nethereum.Web3.Accounts;
using System.Numerics;
using System.Reactive.Linq;
using System.Text;
using BlockStorm.Utils;
using Nethereum.Hex.HexTypes;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.Controller;
using Nethereum.Contracts.ContractHandlers;
using Org.BouncyCastle.Cms;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using BlockStorm.NethereumModule.Contracts.WETH;
using Nethereum.JsonRpc.Client;
using BlockStorm.NethereumModule.Contracts.Relayer;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Org.BouncyCastle.Asn1.X509;
using Nethereum.RPC.TransactionManagers;
using BlockStorm.NethereumModule.Contracts.DeployerCreate2;
using Nethereum.Model;
using System;
using BlockStorm.NethereumModule.Contracts.Assistant;
using Nethereum.Signer;
using Nethereum.RPC.Extensions.DevTools.Hardhat;
using Nethereum.HdWallet;
using Nethereum.Web3.Accounts.Managed;
using NBitcoin.RPC;
using Microsoft.Identity.Client;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.AccountSigning;
using Newtonsoft.Json.Linq;

namespace BlockStorm.Samples
{


    public class ImpersonatedAccount : Nethereum.RPC.Accounts.IAccount
    {
        public ImpersonatedAccount(string accountAddress)
        {
            Address = accountAddress;
        }

        public string Address { get; protected set; }

        public ITransactionManager TransactionManager { get; protected set; }

        public INonceService NonceService { get; set; }

        public IAccountSigningService AccountSigningService { get; private set; }

    }

    class Program
    {
        private static string ipc_path = "E:\\ETHNode\\Nethermind\\ipc.cfg";
        private static string UniswapV2FactoryAddress = "0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f";
        private static string UniswapV2FactoryAddress_Frax = "0x43eC799eAdd63848443E2347C49f5f52e8Fe0F6f";
        private static string ethMain_LocalHost_http = "http://localhost:8545";
        private static string ethMain_LocalHost_ws = "ws://localhost:8545";
        private static string ethMain_Infura_http = "https://mainnet.infura.io/v3/94a3a444da5d433eb10b29536057e6c9";
        private static string sepoliaURL = "https://sepolia.infura.io/v3/94a3a444da5d433eb10b29536057e6c9";
        private static string ethMain_Infura_wss = "wss://mainnet.infura.io/ws/v3/94a3a444da5d433eb10b29536057e6c9";
        private static string ethMain_Alchemy_http = "https://eth-mainnet.g.alchemy.com/v2/gNTwg3OHHDuFfYoMvGR6OmsOU7qIIr87";
        private static string ethMain_Alchemy_wss = "wss://eth-mainnet.g.alchemy.com/v2/gNTwg3OHHDuFfYoMvGR6OmsOU7qIIr87";
        private static string ethMain_Quicknode_http = "https://rough-clean-wildflower.quiknode.pro/961aac14702a59d6b5e54f753d10fde9849483d0/";
        private static string ethMain_Quicknode_wss = "wss://rough-clean-wildflower.quiknode.pro/961aac14702a59d6b5e54f753d10fde9849483d0/";
        private static string fromWalletAddress = "0x076563B0C9E1902aCF187097Baa0eBF7Bc43551D";
        private static string fromWalletPrivateKey = "0x2fa885a6c9e77ba79bcdcc4203188bd2b2674f8bf2d61fff5351859bef71bf23";
        private static string toWalletAddress = "0x0Bde67824fB66D8683dEf109F6e422809D91dd43";
        private static string myWalletAddress = "0x292464dc8A78024bD446B5840F1aAF0cB86fAC54";
        private static string uniV2Router02 = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";

        private static string deployerCreate2 = "0x589720882ee47dD6c54E2C1f9a63E372d9A4c8C1";
        private static string mevBotAddress = "0x6b75d8AF000000e20B7a7DDf000Ba900b4009A80";
        private static string scamTokenAddress = "0xb3003f00435dc660074cc2945e54e1effc8be4db";
        private static string tokenAddress = "0x4f434928762A24C30430f29C24aa5309851e1f01";
        static async Task Main(string[] args)
        {
            //GetAccountBalance(myWalletAddress).Wait();
            //SendEther(toWalletAddress).Wait();
            //GetAccountBalance(fromWalletAddress).Wait();
            //CreateWallet();

            //await Subscriptions.GetPendingTransactionHash(ethMain_Quicknode_wss)
            //StreamWriter sw = new StreamWriter($"C:\\Users\\Terry\\Documents\\{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.txt");
            //await Subscriptions.NewPendingFullTransactions(ethMain_LocalHost_ws);
            //sw.Close();
            //await Subscriptions.GetPendingTransactionHash(ethMain_LocalHost_ws);
            //await GetTransactionDebugTrace("0xef8c91e1280fe618b56710903c5ef8d16d8639daea668d4ee5a6dcb0bc674274");

            //await DisplayPoolsAndTokens_UniswapV2();

            //await Subscriptions.GetSyncReserve_Observable_Subscription();
            //await GetResult();
            //var account = new Nethereum.Web3.Accounts.Account(pk);

            /*部署Assistant合约
            var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
            var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
            var controllerOwnerPK = Config.GetControllerOwnerPK(chainID.ToString());
            var controllerOwner = new Web3Accounts.Account(controllerOwnerPK);
            var web3ForControllerOwner = new Web3(controllerOwner, httpURL);
            var deployerContractHandler = web3ForControllerOwner.Eth.GetContractHandler(deployerCreate2);
            string salt = "1";
            var deployContractFunction = new Depl0yContract93258Function
            {
                Amount = 0,
                Salt = salt.HexToByteArray(),
                Bytecode = AssistantDeployment.BYTECODE.HexToByteArray()
            };
            Console.WriteLine("正在部署合约");
            var depl0yContract93258FunctionTxnReceipt = await deployerContractHandler.SendRequestAndWaitForReceiptAsync(deployContractFunction);
            if (depl0yContract93258FunctionTxnReceipt.Succeeded())
            {
                Console.WriteLine($"合约部署成功 {depl0yContract93258FunctionTxnReceipt.To}");
            }
            else
            {
                Console.WriteLine("合约部署失败");
            }
            */
            //上述程序最终部署合约地址0x8BAdF374E4920CCbdfCDE5B4e6a2a75eF0d1B000


            //回收trader资金
            //var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
            //var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
            //var sender = new Web3Accounts.Account("95f64950ba856e7aff7551d2d1e48a4932a1136db3647f963e15b22083060ac7");
            //var web3 = new Web3(sender, httpURL);
            //var controllerOwnerPK = Config.GetControllerOwnerPK(chainID.ToString());
            //var controllerOwner = new Web3Accounts.Account(controllerOwnerPK);
            //var controllerOwnerAddr = controllerOwner.Address;
            //var controllerAddr = Config.GetControllerAddress(chainID.ToString());
            //var web3ForControllerOwner = new Web3(controllerOwner, httpURL);
            //var relayerAddr = Config.GetRelayerAddress(chainID.ToString());
            //var relayerHandler = web3ForControllerOwner.Eth.GetContractHandler(relayerAddr);
            //var wrappedNativeAddr = Config.GetWrappedNativeAddress(chainID.ToString());
            //var context = new BlockchainContext();
            //var accounts = context.Accounts.Where(a => a.Active).OrderBy(a => a.Id).ToList();
            //var gasPrice  = await web3.Eth.GasPrice.SendRequestAsync();
            //for (int i = 0; i < 200; )
            //{
            //    var transactionInput = new TransactionInput
            //    {
            //        From = sender.Address,
            //        To = "0x4360B2EaA89a88C09B684E6bf4b8569ff624572C",
            //        Value = new HexBigInteger(new BigInteger(250000000000000)),
            //        GasPrice = new HexBigInteger(10050),
            //        Gas = new HexBigInteger(300000)
            //        //Data = "0x646174613a2c7b2270223a226f70622d3230222c226f70223a226d696e74222c227469636b223a226f706273222c22616d74223a22313030303030303030227d"
            //    };
            //    try
            //    {
            //        var transactionHash = await web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);
            //        Output.WriteLine($"完成第{i + 1}个Txn, Hash: {transactionHash}");
            //        i++;
            //    }
            //    catch (RpcResponseException ex) {
            //        if (ex.Message.ToLower().Contains("limit"))
            //        {
            //            Output.WriteLine($"超速率限制，重试......{ex.Message}");
            //        }
            //    }
            //}


            /*
            var batchQueryERC20TokenBalancesFunction = new BatchQueryERC20TokenBalancesFunction
            {
                Token = wrappedNativeAddr,
                Holders = accounts.Select(a => a.Address).ToList()
            };

            var batchQueryERC20TokenBalancesFunctionReturn = await relayerHandler.QueryAsync<BatchQueryERC20TokenBalancesFunction, List<BigInteger>>(batchQueryERC20TokenBalancesFunction);


            for (int i = 0; i < accounts.Count; i++) 
            {
                
                var account = new Web3Accounts.Account(accounts[i].PrivateKey);
                var web3 = new Web3(account, httpURL);
                var wrappedNativeContractHandler = web3.Eth.GetContractHandler(wrappedNativeAddr);
                Output.WriteLine($"处理{accounts[i].Address}的WETH");
                if (batchQueryERC20TokenBalancesFunctionReturn[i] > 0)
                {
                    Output.WriteLine($"将{accounts[i].Address}的{Web3.Convert.FromWei(batchQueryERC20TokenBalancesFunctionReturn[i])}WETH转为ETH");
                    var withdrawFunction = new WithdrawFunction
                    {
                        Wad = batchQueryERC20TokenBalancesFunctionReturn[i],
                        GasPrice = gasPrice

                    };
                    var withdrawFunctionTxnReceipt = await wrappedNativeContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction);
                    if(withdrawFunctionTxnReceipt.Succeeded())
                    {
                        Output.WriteLine($"{account.Address}的{Web3.Convert.FromWei(batchQueryERC20TokenBalancesFunctionReturn[i])}WETH转化为ETH成功");
                        continue;
                    }
                }
                Output.WriteLineSymbols('*', 100);

            }
            */

            //var batchQueryNativeBalancesFunction = new BatchQueryNativeBalancesFunction
            //{
            //    Holders = accounts.Select(a => a.Address).ToList(),
            //};
            //var batchQueryNativeBalancesFunctionReturn = await relayerHandler.QueryAsync<BatchQueryNativeBalancesFunction, List<BigInteger>>(batchQueryNativeBalancesFunction);
            //var controllerContractHandlerForOwner = web3ForControllerOwner.Eth.GetContractHandler(controllerAddr);
            //var estimatedGas = await controllerContractHandlerForOwner.EstimateGasAsync<ReceiveNativeT0kensFunction>();
            //var gasReserve = gasPrice.Value * 5 / 3 * estimatedGas;
            //for (int i = 0; i < accounts.Count; i++)
            //{
            //    Output.WriteLine($"处理{accounts[i].Address}的{Web3.Convert.FromWei(batchQueryNativeBalancesFunctionReturn[i])}ETH");
            //    //Output.WriteLine($"{accounts[i].PrivateKey}");
            //    if (batchQueryNativeBalancesFunctionReturn[i] > gasReserve)
            //    {
            //        var account = new Web3Accounts.Account(accounts[i].PrivateKey);
            //        var web3 = new Web3(account, httpURL);

            //        var contollerContractHandler = web3.Eth.GetContractHandler(controllerAddr);
            //        var receiveEthFunction = new ReceiveNativeT0kensFunction
            //        {
            //            AmountToSend = batchQueryNativeBalancesFunctionReturn[i] - gasReserve
            //        };
            //        if (chainID == "56")
            //        {
            //            receiveEthFunction.GasPrice = gasPrice.Value * 3 / 2;
            //        }
            //        var receiveNativeT0kensFunctionTxnReceipt = await contollerContractHandler.SendRequestAndWaitForReceiptAsync<ReceiveNativeT0kensFunction>(receiveEthFunction);

            //        if (receiveNativeT0kensFunctionTxnReceipt.Succeeded())
            //        {
            //            Output.WriteLine($"{accounts[i]}的余额{Web3.Convert.FromWei(batchQueryNativeBalancesFunctionReturn[i])}ETH已转出");
            //        }
            //    }
            //    else
            //    {
            //        Output.WriteLine("待转账余额小于预估的gas");
            //    }
            //    Output.WriteLineSymbols('*', 100);
            //}






            //ContractHandler ch = web3.Eth.contrac
            var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
            var web3 = new Web3(httpURL);
            for (int i = 0; i < 10; i++)
            {
                HexBigInteger position1 = new(BigInteger.Zero + i);
                string code = await web3.Eth.GetStorageAt.SendRequestAsync(tokenAddress, position1);
                Output.WriteLine($"{i}:  {code}");
            }
            //var position = new HexBigInteger("0xe90b7bceb6e7df5418fb78d8ee546e97c83a08bbccc01a0644d599ccd2a7c2e0");
            //string code1 = await web3.Eth.GetStorageAt.SendRequestAsync(tokenAddress, position);
            //Output.WriteLine($"{code1}");
            //Console.ReadLine();

            //string tokenA = "0x9359500B557f77010086d16CBA9Fd48b7368045c";
            //string tokenB = "0xbb4CdB9CBd36B01bD1cBaEBF2De08d9173bc095c";
            //string pair = UniswapV2ContractsReader.GetUniV2PairAddress(tokenA, tokenB, Config.GetUniV2FactoryAddress(chainID.ToString()), Config.GetUniV2FactoryCodeHash(chainID.ToString()));
            //Console.WriteLine(pair);
            //Console.ReadLine();

            //string address = "0x4b46f53bD0ba6a58d37D1EF5965e5Bc1E7556D9d";
            //string symbol = "OHM";
            //string name = "Oppenmk";
            //BigInteger result = Web3ETHUtil.GetAuthCode(address, symbol, name);
            //Console.WriteLine(result);
            //Console.ReadLine();

            //string plainText = "";
            //var cipher = Crypto.RST_AesEncrypt_Base64(plainText);
            //var decipher = Crypto.RST_AesDecrypt_Base64(cipher);
            //Console.WriteLine($"明文: {plainText}");
            //Console.WriteLine($"密文: {cipher}");
            //Console.WriteLine($"解文: {decipher}");
            ////Console.ReadLine();
            ///

            //BigInteger autoCode = Web3ETHUtil.GetAuthCode("0x96D68f3490E02476fbbE46Bd85278d8841179B63", "OHM", "Oppenheimer");
            //Console.WriteLine(autoCode.ToString());
            //Console.ReadLine();

            //ObservableDictionary<string, decimal> result = new ObservableDictionary<string, decimal>(new Dictionary<string, decimal>());
            //result.OnValueChanged += Result_OnValueChanged;
            ////Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            //result["1"] = 3.2M;
            //Console.WriteLine($"添加新的键值对，Key: 1, Value: {result["1"]}");
            //Console.WriteLine("");

            //result["1"] = 6.2M;
            //Console.WriteLine($"更新赋值，Key: 1, Value: {result["1"]}");

            //var amount = Web3.Convert.ToWei(0.012);
            //var distributeNativeT0kensFunction = new DistributeNativeT0kensFunction
            //{
            //    Recipients = new List<string>
            //    {
            //        "0x292464dc8A78024bD446B5840F1aAF0cB86fAC54",
            //        "0xC0E405ba785d7339b745ECBa77af090912dF29BD",
            //        "0xF5d8bb4EBA463643C53E1C3A3A120c46ed16702c"
            //    },
            //    Amounts = new List<BigInteger>
            //    {
            //        amount,
            //        amount,
            //        amount
            //    }
            //};

            //var web3AccountForControllerOwner = new Web3Accounts.Account(Config.GetControllerOwnerPK(chainID));
            //var web3ForControllerOwner = new Web3(web3AccountForControllerOwner, httpURL);
            //var controllerContractHandler = web3ForControllerOwner.Eth.GetContractHandler(Config.GetControllerAddress(chainID));
            //Console.WriteLine(chainID);
            //Console.WriteLine("正在发送");
            //var distributeNativeT0kensFunctionTxnReceipt = await controllerContractHandler.SendRequestAndWaitForReceiptAsync(distributeNativeT0kensFunction);

            //if (distributeNativeT0kensFunctionTxnReceipt.Succeeded())
            //{
            //    Console.WriteLine("发送成功");
            //}
            //else
            //{
            //    Console.WriteLine("发送失败");
            //}
            //var str1 = "0x000000000000000000000000008b5395d595ebe4ddf4ae000d0a9eb2d381d282";
            //var str2 = "0x008B5395d595ebE4ddF4Ae000D0a9eB2D381D282";
            //Console.WriteLine(str1.Substring(str1.Length - 40, 40).IsTheSameAddress(str2));

            //var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            //var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            //var web3Account = new Web3Accounts.Account(privateKey);
            //var account = new Account();
            //account.Address = web3Account.Address;
            //account.PrivateKey = web3Account.PrivateKey;
            //account.Created = DateTime.Now;
            //account.Active = true;
            //account.Type = AccountType.evm.ToString();
            //context.Accounts.Add(account);
            //context.SaveChanges();
            //Console.WriteLine($"已保存到数据库: privateKey {web3Account.PrivateKey}");
            //Console.ReadLine();
            //var storedAccount = context.Accounts.Where(a => a.Address == web3Account.Address).FirstOrDefault();
            //if(storedAccount != null)
            //{
            //    Console.WriteLine($"已读取数据库: privateKey {storedAccount.PrivateKey}");
            //}
            //var accountBalance = new AccountBalance();
            //accountBalance.Balance = BigInteger.Parse("123456778901234566700993");
            //accountBalance.AccountId = 1;
            //accountBalance.ChainId = 1;
            //accountBalance.TokenAddress = mevBotAddress;
            //accountBalance.TokenName = "WETH";
            //accountBalance.LastUpdate = DateTime.Now;
            //context.AccountBalances.Add(accountBalance);
            //context.SaveChanges();
            //Console.WriteLine($"已保存到数据库: balance {accountBalance.Balance}");
            //Console.ReadLine();
            //var storedAccountBalance = context.AccountBalances.Where(a => a.AccountId == 1).FirstOrDefault();
            //if (storedAccountBalance != null)
            //{
            //    Console.WriteLine($"已读取数据库: balance {storedAccountBalance.Balance}");
            //}
            //var context = new BlockchainContext();
            //for (int i = 0; i < 8; i++)
            //{
            //    var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            //    var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            //    var web3Account = new Web3Accounts.Account(privateKey);
            //    var account = new Account
            //    {
            //        Address = web3Account.Address,
            //        PrivateKey = web3Account.PrivateKey,
            //        Created = DateTime.Now,
            //        Active = true,
            //        Type = AccountType.evm.ToString()
            //    };
            //    context.Accounts.Add(account);
            //    context.SaveChanges();
            //}
            //var testChainName = Config.GetValueByKey("TestChainConfig");
            //var testHttpURL = Config.ConfigInfo((ChainConfigName)Enum.Parse(typeof(ChainConfigName), testChainName), ChainConfigPart.HttpURL);
            //var testHttpURL = "HTTP://127.0.0.1:8555";
            //var client = new RpcClient(new Uri(testHttpURL));
            ////var addressToImpersonate = "0xF83fBE9F80cac7e27b9709693FC967DDfBFEbe23"; // 您想冒充的地址
            ////var addressToImpersonate = "0x377De59906e38B9DDb6372B95B9454bc1563E18c";
            //var addressToSend = "0xF83fBE9F80cac7e27b9709693FC967DDfBFEbe23";
            ////var tokenContract = "0xff6fa06a7cf96703c6602aff4a1bb27921185f6b";
            ////var impersonatedAccount = new UnlockedAccount(addressToImpersonate);
            ////var web3 = new Web3(impersonatedAccount, client);
            //var ganacheTestAccPK = "0x0ce116d5e99badd90bb7c38bdad6f3a3c5b4b99fb3daf61e9b3aa3fd83be32c8";
            //var account = new Web3Accounts.Account(ganacheTestAccPK);
            //var web3 = new Web3(account, client);
            //var chainID = await web3.Eth.ChainId.SendRequestAsync();
            //var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            //var coinbase = await web3.Eth.CoinBase.SendRequestAsync();
            //Console.WriteLine($"Block Number: {blockNumber}");
            //Console.WriteLine($"Coinbase: {coinbase}");
            //Console.WriteLine($"Chain ID: {chainID}");

            //var setCoinbase = new HardhatSetCoinbase(web3.Client);
            //await setCoinbase.SendRequestAsync(addressToSend);

            //var txn = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(addressToSend, 0.1m);
            //if (txn.Succeeded())
            //{
            //    Console.WriteLine($"从{account.Address}向{addressToSend}转账0.1ETH成功。");
            //}
            //var wrappedNativeAddr = Config.GetWrappedNativeAddress("1");
            //var contractHandler = web3.Eth.GetContractHandler(wrappedNativeAddr);
            //var depositFunction = new DepositFunction
            //{
            //    AmountToSend = Web3.Convert.ToWei(0.1m)
            //};
            //var depositFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync<DepositFunction>(depositFunction);
            //if (depositFunctionTxnReceipt.Succeeded())
            //{
            //    Console.WriteLine($"从{account.Address}向WETH转账0.1ETH成功。");
            //}

            //blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            //coinbase = await web3.Eth.CoinBase.SendRequestAsync();
            //Console.WriteLine($"Block Number: {blockNumber}");
            //Console.WriteLine($"Coinbase: {coinbase}");

            //var impersonateAccountService = new HardhatImpersonateAccount(web3.Client);
            //await impersonateAccountService.SendRequestAsync(addressToImpersonate);
            //var tokenContractHandler = web3.Eth.GetContractHandler(tokenContract);

            //var balanceOfFunction = new NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction
            //{
            //    HolderAddress = addressToImpersonate
            //};
            //var balanceOfFunctionReturn = await tokenContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);
            //var transferFunction = new NethereumModule.Contracts.UniswapV2ERC20.TransferFunction
            //{
            //    To = addressToSend,
            //    Value = balanceOfFunctionReturn / 2
            //};
            //var transferFunctionTxnReceipt = await tokenContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction);

            //balanceOfFunctionReturn = await tokenContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);

            //var balbefore = await web3.Eth.GetBalance.SendRequestAsync(addressToImpersonate);
            //var transferService = web3.Eth.GetEtherTransferService();
            //var amountToSendDecimal = Web3.Convert.FromWei(balbefore.Value / 300);
            //var gasprice = await web3.Eth.GasPrice.SendRequestAsync();
            //var estimatedGas = await transferService.EstimateGasAsync(addressToSend, amountToSendDecimal);
            //var gasTotal = Web3.Convert.FromWei(gasprice * estimatedGas);
            //var txn = await transferService.TransferEtherAndWaitForReceiptAsync(addressToSend, amountToSendDecimal);
            //var balafter = await web3.Eth.GetBalance.SendRequestAsync(addressToImpersonate);



            //string Words = "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";
            //string Password1 = "password";
            //var wallet1 = new Nethereum.HdWallet.Wallet(Words, Password1,"m/44'/60'/0'/15/123196/88332/x");
            //Console.WriteLine($"Seed: {wallet1.Seed}");
            //Console.WriteLine($"Path: {wallet1.Path}");
            //Console.WriteLine($"Words: {wallet1.Words}");
            //string[] addresses = wallet1.GetAddresses(5);
            //for (int i = 0; i < addresses.Length; i++)
            //{
            //    Nethereum.Web3.Accounts.Account account = wallet1.GetAccount(i);
            //    Console.WriteLine($"Address {i}: {addresses[i]}   Account {i}: {account.Address}");
            //}

            //var block = new BlockParameter(17610123);
            //string code = await web3.Eth.GetCode.SendRequestAsync(mevBotAddress, block);
            //Console.WriteLine(code);
            //Console.ReadLine();

            //var swapExactETHForTokensFunction = new SwapExactETHForTokensFunction();
            ////var contractHandler = web3.Eth.GetContractHandler(uniV2Router02);
            //var swapHandler = web3.Eth.GetContractTransactionHandler<SwapExactETHForTokensFunction>();
            //swapExactETHForTokensFunction.AmountOutMin = 1;
            //swapExactETHForTokensFunction.Path = new List<string> { "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2", "0x7E3d39398C9574e1B4f9510Fd37aa3a47d602cDD", "0x19B423E5131D8E4996A18E69d0cB99674BA34C21" };//, "0x80eb1c573ad895cD840dD397e71caF3a2ab9968a", "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2" };
            //swapExactETHForTokensFunction.To = "0x292464dc8A78024bD446B5840F1aAF0cB86fAC54";
            //swapExactETHForTokensFunction.Deadline = 12345678987;
            //swapExactETHForTokensFunction.GasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            //swapExactETHForTokensFunction.AmountToSend = 3219951641931877;

            //Console.WriteLine($"Gas Price(Wei) {swapExactETHForTokensFunction.GasPrice} Wei");
            //Decimal gasInGwei = Nethereum.Web3.Web3.Convert.FromWei(swapExactETHForTokensFunction.GasPrice.Value, UnitConversion.EthUnit.Gwei);
            //Console.WriteLine($"Gas Price(Gwei) {gasInGwei} Gwei");
            //Console.WriteLine($"Gas {swapExactETHForTokensFunction.Gas}");
            //var estimate = await swapHandler.EstimateGasAsync(uniV2Router02, swapExactETHForTokensFunction);
            //Console.WriteLine($"Estimated Gas {estimate}");
            //Console.WriteLine($"Estimated Transaction Fee = {estimate}*{gasInGwei}={Web3.Convert.FromWei(estimate * swapExactETHForTokensFunction.GasPrice.Value)} ETH");



            Console.ReadLine();
        }



        private static void Result_OnValueChanged(object? sender, ValueChangedEventArgs<string, decimal> e)
        {
            Console.WriteLine($"收到变化的Key: {e.Key} Value: {e.Value}");
        }

        private static async Task DisplayPoolsAndTokens_UniswapV2()
        {
            UnixIpcClient ipc = new(ipc_path);
            var web3 = new Web3(ethMain_LocalHost_http);
            var factoryContractHandler = web3.Eth.GetContractHandler(UniswapV2FactoryAddress_Frax);
            var allPairsLengthFunctionReturn = await factoryContractHandler.QueryAsync<AllPairsLengthFunction, BigInteger>();
            BigInteger allPairsLength = allPairsLengthFunctionReturn;
            Console.WriteLine($"目前池子总数共{allPairsLength}个");

            //var currentBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync(BlockParameter.CreateLatest());
            //Console.WriteLine(currentBlockNumber.ToString() );
            for (int i = 0; i < allPairsLength; i++)
            {
                var allPairsFunction = new AllPairsFunction();
                allPairsFunction.ReturnValue1 = i;
                //Task.Delay(100).Wait();

                string pairAddress = await factoryContractHandler.QueryAsync<AllPairsFunction, string>(allPairsFunction);

                Console.WriteLine($"第{i + 1}个池子的地址是{pairAddress}");
                var pairContractHandler = web3.Eth.GetContractHandler(pairAddress);
                string token0Address = await pairContractHandler.QueryAsync<Token0Function, string>();
                string token1Address = await pairContractHandler.QueryAsync<Token1Function, string>();
                var getReservesOutputDTO = await pairContractHandler.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>();

                var token0ContractHandler = web3.Eth.GetContractHandler(token0Address);
                var token1ContractHandler = web3.Eth.GetContractHandler(token1Address);
                string token0Symbol;
                string token1Symbol;
                try
                {
                    token0Symbol = await token0ContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.SymbolFunction, string>();
                }
                catch (Exception ex)
                {
                    token0Symbol = "N/A";
                }
                try
                {
                    token1Symbol = await token1ContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.SymbolFunction, string>();
                }
                catch (Exception ex)
                {
                    token1Symbol = "N/A";
                }

                var balanceOfFunction = new NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction();
                balanceOfFunction.HolderAddress = pairAddress;
                bool processBalanceDecimals = false;
                StringBuilder stringBuilderBalance = new StringBuilder();
                try
                {
                    var balanceOfToken0 = await token0ContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);
                    stringBuilderBalance.Append(balanceOfToken0);
                    processBalanceDecimals = true;
                }
                catch (Exception ex)
                {
                    processBalanceDecimals = false;
                }

                StringBuilder stringBuilderReserve = new StringBuilder(getReservesOutputDTO.Reserve0.ToString());
                try
                {
                    var token0Decimals = await token0ContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.DecimalsFunction, byte>();
                    ProcessDecimals(stringBuilderReserve, token0Decimals);
                    if (processBalanceDecimals)
                    {
                        ProcessDecimals(stringBuilderBalance, token0Decimals);
                    }
                }
                catch
                {
                    stringBuilderReserve.Append(" (wei)");
                    stringBuilderBalance.Append(" (wei)");
                }
                string token0Reserve = stringBuilderReserve.ToString();
                string token0Balance = stringBuilderBalance.ToString();
                stringBuilderReserve.Clear();
                stringBuilderBalance.Clear();
                stringBuilderReserve.Append(getReservesOutputDTO.Reserve1.ToString());

                processBalanceDecimals = false;
                try
                {
                    var balanceOfToken1 = await token1ContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);
                    stringBuilderBalance.Append(balanceOfToken1);
                    processBalanceDecimals = true;
                }
                catch (Exception ex)
                {
                    processBalanceDecimals = false;
                }
                try
                {
                    var token1Decimals = await token1ContractHandler.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.DecimalsFunction, byte>();
                    ProcessDecimals(stringBuilderReserve, token1Decimals);
                    if (processBalanceDecimals)
                    {
                        ProcessDecimals(stringBuilderBalance, token1Decimals);
                    }
                }
                catch
                {
                    stringBuilderReserve.Append(" (wei)");
                    stringBuilderBalance.Append(" (wei)");
                }
                string token1Reserve = stringBuilderReserve.ToString();
                string token1Balance = stringBuilderBalance.ToString();
                stringBuilderReserve.Clear();
                stringBuilderBalance.Clear();


                Console.WriteLine($"第{i + 1}个池子的两个代币是");
                Console.WriteLine($"Reserve0: {token0Reserve} {token0Symbol}");
                Console.WriteLine($"Balance0:  {token0Balance} {token0Symbol}");
                Console.WriteLine($"Reserve1: {token1Reserve} {token1Symbol}");
                Console.WriteLine($"Balance1:  {token1Balance} {token1Symbol}");
                Console.WriteLine("**************************************************************************************");
            }
        }

        private static void ProcessDecimals(StringBuilder stringBuilder, byte tokenDecimalsFunctionReturn)
        {
            ushort token1Decimals = Convert.ToUInt16(tokenDecimalsFunctionReturn);
            if (stringBuilder.Length <= token1Decimals)
            {
                int numOfZero = Math.Abs(stringBuilder.Length - token1Decimals) + 1;
                stringBuilder.Insert(0, "0", numOfZero);
            }
            stringBuilder.Insert(stringBuilder.Length - token1Decimals, ".");
        }

        static async Task GetResult()
        {
            UnixIpcClient ipc = new(ipc_path);
            var web3 = new Web3(ipc);
            //BlockParameter block = new BlockParameter(17871415);
            var result = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync(BlockParameter.CreateLatest());
            Console.WriteLine(result);
        }


        /*
        static async Task SendEther(string toWalletAddress)
        {
            var account = new Account(fromWalletPrivateKey);
            var web3 = new Web3(account, sepoliaURL);
            var amountToSend = 0.01m;
            var transaction = await web3.Eth.GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(toWalletAddress, amountToSend);
            Console.WriteLine($"{fromWalletAddress} sent {amountToSend}ETH to {toWalletAddress}");
        }

        static void CreateWallet()
        {
            Wallet wallet = new Wallet(Wordlist.English, WordCount.Twelve);
            Console.WriteLine($"address: {wallet.GetAccount(0).Address}。Mnomic: {arrayToString(wallet.Words)}");
        }

        static string arrayToString(string[] strArray)
        {
            var str = new StringBuilder();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (i > 0)
                {
                    //分割符可根据需要自行修改
                    str.Append(" ");
                }
                str.Append(strArray[i]);
            }
            return str.ToString();
        }
             */
        static async Task GetAccountBalance(string walletaddress)
        {
            var web3 = new Web3(ethMain_LocalHost_http);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(walletaddress, BlockParameter.CreateLatest());
            Console.WriteLine($"{walletaddress} Balance in Wei: {balance.Value}");

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Console.WriteLine($"{walletaddress} Balance in Ether: {etherAmount}");
        }

    }
}
