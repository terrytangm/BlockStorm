using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.Controller;
using BlockStorm.Utils;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Web3Account = Nethereum.Web3.Accounts;

namespace BlockStorm.Infinity.CampaignManager
{
    public partial class DepolyContractForm : Form
    {
        public DepolyContractForm()
        {
            InitializeComponent();
        }
        public long? chainID;
        public string? chainName;
        private BlockchainContext context;
        private long? deployerID;
        public string? httpURL;
        private UniswapV2ContractsReader? uniswapV2Reader;

        public string wrappedNativeAddr;
        public string controllerAddr;
        public Web3Account.Account controllerOwnerAccount;

        public BigInteger gasPrice;

        private Web3 web3;

        private void BtnCopyDeployer_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtDeployer.Text);
            MessageBox.Show("部署者地址复制成功！");
        }

        private void BtnCopyPK_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtPK.Text);
            MessageBox.Show("部署者私钥复制成功！");
        }

        private void BtnGeneratorDeployer_Click(object sender, EventArgs e)
        {
            Web3Account.Account deployerAccount = Web3ETHUtil.GenerateNewWeb3Account();
            txtDeployer.Text = deployerAccount.Address;
            txtPK.Text = deployerAccount.PrivateKey;
            var context = new BlockchainContext();
            var account = new Account
            {
                Address = deployerAccount.Address,
                PrivateKey = deployerAccount.PrivateKey,
                Created = DateTime.Now,
                Active = false,
                Type = AccountType.evm.ToString()
            };
            context.Accounts.Add(account);
            context.SaveChanges();
            btnGeneratorDeployer.Enabled = false;
            MessageBox.Show("部署者已生成并保存到数据库！");
            BtnSaveContract.Enabled = true;
            btnSendFunds.Enabled = true;
            deployerID = account.Id;
            txtDeployerBalance.Text = "0";
            web3 = new Web3(controllerOwnerAccount, httpURL);
        }

        private async void DepolyContractForm_Load(object sender, EventArgs e)
        {
            txtChain.Text = chainName;
            context = new BlockchainContext();
            uniswapV2Reader = new UniswapV2ContractsReader(httpURL);
            txtControllerAddress.Text = controllerAddr;
            await RefreshControllerWETHBalance();
            await RefreshControllerETHBalance();
        }
        private async Task RefreshDeployerETHBalance()
        {
            BigInteger deployerETHBalanceInWei = await uniswapV2Reader.GetNativeBalanceInWei(txtDeployer.Text);
            decimal eployerETHBalanceInETH = Web3.Convert.FromWei(deployerETHBalanceInWei);
            txtDeployerBalance.Text = eployerETHBalanceInETH.ToString();
        }

        private async Task RefreshControllerETHBalance()
        {
            BigInteger controllerETHBalanceInWei = await uniswapV2Reader.GetNativeBalanceInWei(controllerAddr);
            decimal controllerETHBalanceInETH = Web3.Convert.FromWei(controllerETHBalanceInWei);
            txtETHBalance.Text = controllerETHBalanceInETH.ToString();
        }

        private async Task RefreshControllerWETHBalance()
        {
            BigInteger controllerWETHBalanceInWei = await uniswapV2Reader.GetTokenBalanceOf(wrappedNativeAddr, controllerAddr);
            decimal controllerWETHBalanceInETH = Web3.Convert.FromWei(controllerWETHBalanceInWei);
            txtWETHBalance.Text = controllerWETHBalanceInETH.ToString();
        }

        private async void BtnSaveContract_ClickAsync(object sender, EventArgs e)
        {
            var token = context.Tokens.Where(t => t.TokenAddress == txtContractAddress.Text.Trim() && t.ChainId == chainID).FirstOrDefault();
            if (token != null)
            {
                token.DeployerID = deployerID;
                token.FuncSig = txtFuncSig.Text.Trim();
                context.SaveChanges();
            }
            else
            {
                token = await uniswapV2Reader.GetTokenModelByAddress(txtContractAddress.Text.Trim(), chainID.Value);
                token.DeployerID = deployerID;
                token.AuthCode = txtAuthCode.Text.Trim();
                token.FuncSig = txtFuncSig.Text.Trim();
                context.Tokens.Add(token);
                context.SaveChanges();
            }
            MessageBox.Show("Token合约已更新到数据库！");
        }

        private async void BtnSendFunds_ClickAsync(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmountTosend.Text, out decimal amountToSendDecimal))
            {
                MessageBox.Show("必须输入数值！");
                return;
            }
            BigInteger amountToSend = Web3.Convert.ToWei(amountToSendDecimal);
            BigInteger controllerETHBalanceInWei = await uniswapV2Reader.GetNativeBalanceInWei(controllerAddr);
            BigInteger controllerWETHBalanceInWei = await uniswapV2Reader.GetTokenBalanceOf(wrappedNativeAddr, controllerAddr);
            var contractHandler = web3.Eth.GetContractHandler(controllerAddr);
            if (amountToSend > controllerETHBalanceInWei)
            {
                if (amountToSend - controllerETHBalanceInWei > controllerWETHBalanceInWei)
                {
                    MessageBox.Show("controller余额不足！");
                    return;
                }
                var withdrawWethToETH50992Function = new WithdrawWethToETH50992Function
                {
                    Amount = amountToSend - controllerETHBalanceInWei
                };
                if (chainID == 56)
                {
                    withdrawWethToETH50992Function.GasPrice = gasPrice;
                }
                var withdrawWethToETH50992FunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(withdrawWethToETH50992Function);
                if (withdrawWethToETH50992FunctionTxnReceipt.Failed())
                {
                    MessageBox.Show("controller的WETH转为ETH出错");
                    return;
                }
            }
            var distributeNativeT0kensFunction = new DistributeNativeT0kensFunction
            {
                Recipients = new List<string>
            {
                txtDeployer.Text
            },
                Amounts = new List<BigInteger>
            {
                amountToSend
            }
            };
            if (chainID == 56)
            {
                distributeNativeT0kensFunction.GasPrice = gasPrice;
            }
            var distributeNativeT0kensFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(distributeNativeT0kensFunction);
            if (distributeNativeT0kensFunctionTxnReceipt.Failed())
            {
                MessageBox.Show("controller的向部署者转账ETH出错!");
                return;
            }
            MessageBox.Show("controller的向部署者转账ETH成功!");
            await RefreshControllerWETHBalance();
            await RefreshControllerETHBalance();
            await RefreshDeployerETHBalance();
        }

        private void BtnCalAuthCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTokenName.Text) || string.IsNullOrEmpty(txtSymbol.Text))
            {
                MessageBox.Show("需要Name和Symbol才可计算授权码");
                return;
            }
            txtAuthCode.Text = Web3ETHUtil.GetAuthCode(controllerAddr, txtSymbol.Text, txtTokenName.Text).ToString();
        }

        private void BtnCopyAuthCode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtAuthCode.Text);
            MessageBox.Show("授权码复制成功！");
        }

        private void BtnGenerateTotalSupply_Click(object sender, EventArgs e)
        {
            var random = new Random();
            var exponet = random.Next(18, 20);
            var value = random.Next(200000, 5000000);
            value -= (value % 10000);
            BigInteger totalSupply = value * BigInteger.Pow(10, exponet);
            txtTotalSupply.Text = totalSupply.ToString();
        }

        private void BtnCopyTotalSupply_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtTotalSupply.Text);
            MessageBox.Show("TotalSupply复制成功！");
        }
    }
}
