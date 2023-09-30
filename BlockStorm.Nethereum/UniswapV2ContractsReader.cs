using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockStorm.NethereumModule.Contracts.UniswapV2Factory;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.NethereumModule.Contracts.UniswapV2ERC20;
using Nethereum.Web3;
using System.Numerics;
using Nethereum.Contracts.ContractHandlers;
using System.Net.Mail;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using BlockStorm.EFModels;
using BlockStorm.NethereumModule.Contracts.Relayer;
using Org.BouncyCastle.Asn1.X509;

namespace BlockStorm.NethereumModule
{
    public class UniswapV2ContractsReader
    {
        private static readonly string uniswapV2FactoryAddress = "0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f";

        public static string GetUniswapV2PairAddress(string tokenA, string tokenB)
        {
            var tokenAHex = new HexBigInteger(tokenA);
            var tokenBHex = new HexBigInteger(tokenB);
            var sha3 = new Sha3Keccack();
            string salt = tokenAHex.Value < tokenBHex.Value ? sha3.CalculateHashFromHex(tokenA, tokenB) : sha3.CalculateHashFromHex(tokenB, tokenA);
            return ContractUtils.CalculateCreate2AddressUsingByteCodeHash(uniswapV2FactoryAddress,
                salt,
                "96e8ac4277198ff8b6f785478aa9a39f403cb768dd02cbee326c3e7da348845f");//initial pair creation code hash
        }

        public static bool IsAddressSmaller(string tokenA, string tokenB)
        {
            var tokenAHex = new HexBigInteger(tokenA);
            var tokenBHex = new HexBigInteger(tokenB);
            return tokenAHex.Value < tokenBHex.Value;
        }

        private readonly Web3 web3;
        public UniswapV2ContractsReader(string httpUrl)
        {
            web3 = new Web3(httpUrl);
        }
        public async Task<BigInteger> GetNativeBalanceInWei(string Holder)
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(Holder);
            return balance;
        }
        public async Task<decimal> GetNativeBalanceInETH(string Holder)
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(Holder);
            return Web3.Convert.FromWei(balance);
        }

        public async Task<decimal> GetWrappedNativeBalanceInETH(string wrappedNative, string holderAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(wrappedNative);
            var balanceOfFunction = new Contracts.UniswapV2ERC20.BalanceOfFunction
            {
                HolderAddress = holderAddress
            };
            BigInteger balanceInWei = await contractHandler.QueryAsync<Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);
            return Web3.Convert.FromWei(balanceInWei);
        }

        public async Task<List<List<BigInteger>>> GetBatchBalances(string queryContractAddress, string wrappedNativeAddreess, List<string> holders)
        {
            var contractHandler = web3.Eth.GetContractHandler(queryContractAddress);
            var batchQueryBalancesFunction = new BatchQueryBalancesFunction
            {
                WrappedNative = wrappedNativeAddreess,
                Holders = holders
            };
            return await contractHandler.QueryAsync<BatchQueryBalancesFunction, List<List<BigInteger>>>(batchQueryBalancesFunction);
        }

        public async Task<BigInteger> GetAllPairsLength(string factoryAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(factoryAddress);
            var allPairsLength = await contractHandler.QueryAsync<AllPairsLengthFunction, BigInteger>();
            return allPairsLength;
        }

        public async Task<string> GetPairAddressByIndex(string factoryAddress, int pairIndex)
        {
            var contractHandler = web3.Eth.GetContractHandler(factoryAddress);
            var allPairsFunction = new AllPairsFunction
            {
                pairIndex = pairIndex
            };
            string pairAddress = await contractHandler.QueryAsync<AllPairsFunction, string>(allPairsFunction);
            return pairAddress;
        }

        public async Task<string> GetToken0Address(string pairAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(pairAddress);
            var token0Address = await contractHandler.QueryAsync<Token0Function, string>();
            return token0Address;
        }

        public async Task<string> GetToken1Address(string pairAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(pairAddress);
            var token1Address = await contractHandler.QueryAsync<Token1Function, string>();
            return token1Address;
        }

        public async Task<GetReservesOutputDTO> GetReserves(string pairAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(pairAddress);
            var getReservesOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>();
            return getReservesOutputDTO;

        }

        public async Task<byte?> GetTokenDecimal(string tokenAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(tokenAddress);
            try
            {
                var decimals = await contractHandler.QueryAsync<Contracts.UniswapV2ERC20.DecimalsFunction, byte>();
                return decimals;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<string> GetTokenSymbol(string tokenAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(tokenAddress);
            try
            {
                var symbol = await contractHandler.QueryAsync<Contracts.UniswapV2ERC20.SymbolFunction, string>();
                return symbol;
            }
            catch 
            {
                return "N/A";
            }
        }

        public async Task<string> GetTokenName(string tokenAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(tokenAddress);
            try
            {
                var name = await contractHandler.QueryAsync<Contracts.UniswapV2ERC20.NameFunction, string>();
                return name;
            }
            catch
            {
                return "N/A";
            }
            
        }

        public async Task<string> GetTokenTotalSupply(string tokenAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(tokenAddress);
            try
            {
                var totalSupply = await contractHandler.QueryAsync<Contracts.UniswapV2ERC20.TotalSupplyFunction, BigInteger>();
                return totalSupply.ToString().Trim();
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<BigInteger> GetTokenBalanceOf(string tokenAddress, string holderAddress)
        {
            var contractHandler = web3.Eth.GetContractHandler(tokenAddress);
            var balanceOfFunction = new Contracts.UniswapV2ERC20.BalanceOfFunction();
            balanceOfFunction.HolderAddress = holderAddress;
            return await contractHandler.QueryAsync<Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);
        }

        public async Task<Token> GetTokenModelByAddress(string tokenAddress, long chainID)
        {
            Token token = new()
            {
                TokenAddress = tokenAddress,
                ChainId = chainID,
                Created = DateTime.Now,
                Decimals = await GetTokenDecimal(tokenAddress),
                Symbol = await GetTokenSymbol(tokenAddress),
                Name = await GetTokenName(tokenAddress),
                TotalSupply = await GetTokenTotalSupply(tokenAddress),
                IsTopToken = false,
                LowestReserve = "0"
            };
            if (token.Name?.Length > 300)
            {
                token.Name = token.Name[..300];
            }
            if (token.Symbol?.Length > 200)
            {
                token.Symbol = token.Symbol[..200];
            }
            return token;
        }
    }
}
